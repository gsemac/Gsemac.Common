﻿using Gsemac.Collections;
using Gsemac.Collections.Extensions;
using Gsemac.IO.Logging;
using Gsemac.Polyfills.System.Threading.Tasks;
using Gsemac.Text.PatternMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    public sealed class RateLimitHandler :
        HttpWebRequestHandler {

        // Public members

        public bool AdaptiveRateLimitingEnabled { get; set; } = false;
        public TimeSpan MaxDelayBetweenRequests { get; set; } = TimeSpan.MaxValue;
        public ICollection<IRateLimitRule> Rules => GetWrappedRules();

        public RateLimitHandler() :
            this(Enumerable.Empty<IRateLimitRule>()) {
        }
        public RateLimitHandler(ILogger logger) :
           this(Enumerable.Empty<IRateLimitRule>(), logger) {
        }
        public RateLimitHandler(IEnumerable<IRateLimitRule> rules) :
            this(rules, Logger.Null) {
        }
        public RateLimitHandler(IEnumerable<IRateLimitRule> rules, ILogger logger) {

            if (rules is null)
                throw new ArgumentNullException(nameof(rules));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = new NamedLogger(logger, nameof(RateLimitHandler));

            this.rules.AddRange(rules.Select(rule => new RateLimitingRuleInfo(rule)));

        }

        // Protected members

        protected override IHttpWebResponse GetResponse(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // Check if we have any rules applying to this request.
            // It's possible for multiple rules to apply to the same request, and we'll take the most restrictive.

            IEnumerable<RateLimitingRuleInfo> matchingRules = GetMatchingRules(request.RequestUri);

            bool rateLimitingRequired = false;
            TimeSpan delay = TimeSpan.Zero;
            TimeSpan requestTimeout = TimeSpan.FromMilliseconds(request.Timeout);

            if (matchingRules.Any()) {

                // We may need to rate limit this request.

                DateTimeOffset currentTime = DateTimeOffset.Now;

                delay = GetMaximumDelay(matchingRules, requestTimeout);

                lock (requestMutex) {

                    DateTimeOffset lastRequestTime = GetMaximumLastRequestTime(matchingRules);

                    if ((currentTime - lastRequestTime) < delay) {

                        // Update the request time to the time this request will end up being sent.

                        foreach (RateLimitingRuleInfo info in matchingRules)
                            info.LastRequestTime = currentTime + delay;

                        rateLimitingRequired = true;

                    }
                    else {

                        // We can make the request immediately and don't need to wait.

                        foreach (RateLimitingRuleInfo info in matchingRules)
                            info.LastRequestTime = currentTime;

                    }

                }

            }

            if (rateLimitingRequired) {

                TimeSpan randomJitter = TimeSpan.FromMilliseconds(randomSource.Next(250));

                TaskEx.Delay(delay + randomJitter, cancellationToken).Wait(cancellationToken);

            }

            try {

                return base.GetResponse(request, cancellationToken);

            }
            catch (WebException ex) {

                if (AdaptiveRateLimitingEnabled && ex.Response is object && new HttpWebResponseAdapter(ex.Response).StatusCode == (HttpStatusCode)429) {

                    // The server responded with "429 Too Many Requests".

                    // We can apply throttling automatically to increase the time between requests.

                    bool isAutoRuleAlreadyDefined = matchingRules.Where(info => !info.IsUserDefined).Any();

                    lock (rulesMutex) {

                        matchingRules = GetMatchingRules(request.RequestUri).Where(info => !info.IsUserDefined);

                        // Only update rules if another request didn't beat us to it.
                        // If there wasn't a rule defined before we entered the block, but now there is, it's already been initialized by another request.

                        bool autoRuleInitializedByDifferentRequest = !isAutoRuleAlreadyDefined && matchingRules.Any();

                        if (!autoRuleInitializedByDifferentRequest) {

                            if (!matchingRules.Any()) {

                                // If there is no existing automatic rule for this endpoint, create one.
                                // Automatic throttling will apply to all paths on the host.

                                string throttledEndpoint = request.RequestUri.GetLeftPart(UriPartial.Authority).TrimEnd('/') + "/";

                                IPatternMatcher newRulePattern = new WildcardPattern(throttledEndpoint + "*");
                                IRateLimitRule newRule = new RateLimitRule(newRulePattern, InitialAutomaticDelay);

                                logger.Info($"Got status code 429, throttling ({newRule.TimeWindow.TotalMilliseconds:#.#}ms): {newRule.Pattern}");

                                rules.Add(new RateLimitingRuleInfo(newRule) {
                                    IsUserDefined = false,
                                });

                            }
                            else {

                                // If there are already automatic rules for this endpoint, increase the time between requests.

                                foreach (RateLimitingRuleInfo autoRuleInfo in matchingRules) {

                                    TimeSpan newDelay = ClampDelay(TimeSpan.FromMilliseconds(autoRuleInfo.Rule.TimeWindow.TotalMilliseconds * AutomaticDelayMultiplier), requestTimeout);

                                    logger.Info($"Got status code 429, throttling ({newDelay.TotalMilliseconds:#.#}ms): {autoRuleInfo.Rule.Pattern}");

                                    autoRuleInfo.Rule = new RateLimitRule(autoRuleInfo.Rule.Pattern, newDelay);

                                }

                            }

                        }

                    }

                }

                // We will allow the exception to propagate without retrying the request.
                // It's up to the client to retry the request with the new rate limit (e.g. with RetryHandler).

                throw;

            }

        }

        // Private members

        private sealed class RateLimitingRuleInfo {

            // Public members

            public IRateLimitRule Rule { get; set; }
            public DateTimeOffset LastRequestTime { get; set; } = DateTimeOffset.MinValue;
            public bool IsUserDefined { get; set; } = true;

            public RateLimitingRuleInfo(IRateLimitRule rule) {

                if (rule is null)
                    throw new ArgumentNullException(nameof(rule));

                Rule = rule;

            }

        }

        private const double AutomaticDelayMultiplier = 1.5;
        private static readonly TimeSpan InitialAutomaticDelay = TimeSpan.FromMilliseconds(500);

        private readonly object rulesMutex = new object();
        private readonly object requestMutex = new object();
        private readonly ICollection<RateLimitingRuleInfo> rules = new List<RateLimitingRuleInfo>();
        private readonly ILogger logger;
        private static readonly Random randomSource = new Random();

        private ICollection<IRateLimitRule> GetWrappedRules() {

            return new ConcurrentCollectionDecorator<IRateLimitRule>(
                new MappedCollectionDecorator<RateLimitingRuleInfo, IRateLimitRule>(rules,
                i => i.Rule,
                i => new RateLimitingRuleInfo(i)), rulesMutex);

        }

        private IEnumerable<RateLimitingRuleInfo> GetMatchingRules(Uri requestUri) {

            if (requestUri is null)
                throw new ArgumentNullException(nameof(requestUri));

            lock (rulesMutex) {

                return rules.Where(info => info.Rule.Pattern?.IsMatch(requestUri.AbsoluteUri) ?? false)
                     .ToArray();

            }

        }
        private TimeSpan ClampDelay(TimeSpan delay, TimeSpan maximumTimeout) {

            return new[] {
                delay,
                MaxDelayBetweenRequests,
                maximumTimeout
            }.Min();

        }
        private TimeSpan GetMaximumDelay(IEnumerable<RateLimitingRuleInfo> matchingRules, TimeSpan maximumTimeout) {

            if (matchingRules is null)
                throw new ArgumentNullException(nameof(matchingRules));

            TimeSpan maxDelay = matchingRules
                .Select(info => info.Rule)
                .Select(rule => TimeSpan.FromMilliseconds(Math.Ceiling(rule.TimeWindow.TotalMilliseconds / rule.MaxRequests)))
                .Max();

            return ClampDelay(maxDelay, maximumTimeout);

        }
        private DateTimeOffset GetMaximumLastRequestTime(IEnumerable<RateLimitingRuleInfo> matchingRules) {

            if (matchingRules is null)
                throw new ArgumentNullException(nameof(matchingRules));

            return matchingRules.Select(info => info.LastRequestTime)
                .Max();

        }

    }

}
using Gsemac.Collections.Extensions;
using Gsemac.Polyfills.System.Threading.Tasks;
using Gsemac.Text.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gsemac.Net.Http {

    public class RateLimitingHandler :
        HttpWebRequestHandler {

        // Public members

        public TimeSpan MaximumDelayBetweenRequests { get; set; } = TimeSpan.MaxValue;
        public ICollection<IRateLimitingRule> Rules => rules;

        public RateLimitingHandler() { }
        public RateLimitingHandler(IEnumerable<IRateLimitingRule> rateLimitingRules) {

            if (rateLimitingRules is null)
                throw new ArgumentNullException(nameof(rateLimitingRules));

            Rules.AddRange(rateLimitingRules);

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // Check if we have any rules applying to this request.
            // It's possible for multiple rules to apply to the same request, and we'll take the most restrictive.

            IEnumerable<IRateLimitingRule> rateLimitingRules = rules.Where(rule => IsRuleMatch(request, rule));

            bool rateLimitingRequired = false;
            TimeSpan delay = TimeSpan.Zero;

            if (rateLimitingRules.Any()) {

                // We may need to rate limit this request.

                DateTimeOffset currentTime = DateTimeOffset.Now;

                delay = GetDelay(request, rateLimitingRules);

                lock (globalRequestMutex) {

                    DateTimeOffset lastRequestTime = GetLastRequestTime(rateLimitingRules);

                    if ((currentTime - lastRequestTime) < delay) {

                        // Update the request time to the time this request will end up being sent.

                        foreach (IRateLimitingRule rule in rateLimitingRules)
                            lastRequestTimes[rule.Endpoint] = currentTime + delay;

                        rateLimitingRequired = true;

                    }
                    else {

                        // We can make the request immediately and don't need to wait.

                        foreach (IRateLimitingRule rule in rateLimitingRules)
                            lastRequestTimes[rule.Endpoint] = currentTime;

                    }

                }

            }

            if (rateLimitingRequired) {

                TimeSpan randomJitter = TimeSpan.FromMilliseconds(randomSource.Next(250));

                TaskEx.Delay(delay + randomJitter, cancellationToken).Wait(cancellationToken);

            }

            return base.Send(request, cancellationToken);

        }

        // Private members

        private readonly ICollection<IRateLimitingRule> rules = new List<IRateLimitingRule>();
        private readonly IDictionary<string, DateTimeOffset> lastRequestTimes = new Dictionary<string, DateTimeOffset>();
        private readonly object globalRequestMutex = new object();
        private static readonly Random randomSource = new Random();

        private TimeSpan GetDelay(IHttpWebRequest request, IEnumerable<IRateLimitingRule> rateLimitingRules) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (rateLimitingRules is null)
                throw new ArgumentNullException(nameof(rateLimitingRules));

            TimeSpan ruleDelay = rateLimitingRules
                .Select(rule => TimeSpan.FromMilliseconds(Math.Ceiling(rule.TimePeriod.TotalMilliseconds / rule.RequestsPerTimePeriod)))
                .Max();

            return new[] {
                ruleDelay,
                MaximumDelayBetweenRequests,
                TimeSpan.FromMilliseconds(request.Timeout)
            }.Min();

        }
        private DateTimeOffset GetLastRequestTime(IEnumerable<IRateLimitingRule> rateLimitingRules) {

            if (rateLimitingRules is null)
                throw new ArgumentNullException(nameof(rateLimitingRules));

            DateTimeOffset lastRequestTime = DateTimeOffset.MinValue;

            foreach (IRateLimitingRule rule in rateLimitingRules) {

                if (lastRequestTimes.TryGetValue(rule.Endpoint, out DateTimeOffset ruleLastRequestTime) && ruleLastRequestTime > lastRequestTime)
                    lastRequestTime = ruleLastRequestTime;

            }

            return lastRequestTime;

        }

        private static bool IsRuleMatch(IHttpWebRequest request, IRateLimitingRule rateLimitingRule) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (rateLimitingRule is null)
                throw new ArgumentNullException(nameof(rateLimitingRule));

            // We will match all paths at the depth of the given endpoint and below.
            // The scheme is ignored unless the endpoint pattern includes a scheme.

            // "//example.com/" should match requests to "https://example.com/some/path"
            // "example.com/" should match requests to "https://example.com/some/path"
            // "example.com" should match requests to "https://example.com/some/path"
            // "https://example.com/" should ONLY match "https://example.com/some/path" but not "http://example.com/some/path"

            if (string.IsNullOrWhiteSpace(rateLimitingRule.Endpoint) || string.IsNullOrWhiteSpace(request.RequestUri.AbsoluteUri))
                return false;

            string endpointPattern = rateLimitingRule.Endpoint.Trim().TrimEnd('/');
            string requestUri = request.RequestUri.AbsoluteUri.Trim().TrimEnd('/');

            // Strip the scheme from the request URI as dictated by the endpoint pattern.

            if (endpointPattern.StartsWith("//")) {

                // Make it so both strings begin with "//".

                requestUri = requestUri.After(":");

            }
            else if (!endpointPattern.Contains("://")) {

                // If the pattern does not specify a scheme, we'll strip the scheme from the URI.

                requestUri = requestUri.After("://");

            }

            return requestUri.Equals(endpointPattern, StringComparison.OrdinalIgnoreCase) ||
                requestUri.StartsWith(endpointPattern + "/");

        }

    }

}
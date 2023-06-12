using Gsemac.Collections;
using Gsemac.Polyfills.System.Threading.Tasks;
using Gsemac.Text.PatternMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gsemac.Net.Http {

    public class RateLimitingHandler :
        HttpWebRequestHandler {

        // Public members

        public TimeSpan MaximumDelayBetweenRequests { get; set; } = TimeSpan.MaxValue;
        public ICollection<IRateLimitingRule> Rules => rules;

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // Check if we have any rules applying to this request.

            IRateLimitingRule rateLimitingRule = rules.Where(rule => new WildcardPattern(rule.Endpoint).IsMatch(request.RequestUri.AbsoluteUri))
                .FirstOrDefault();

            bool rateLimitingRequired = false;
            TimeSpan delay = TimeSpan.Zero;

            if (rateLimitingRule is object) {

                // We may need to rate limit this request.

                DateTimeOffset currentTime = DateTimeOffset.Now;

                delay = GetDelay(request, rateLimitingRule);

                lock (globalRequestMutex) {

                    if (lastRequestTimes.TryGetValue(rateLimitingRule.Endpoint, out DateTimeOffset lastRequestTime) && lastRequestTime < currentTime + delay) {

                        // Update the request time to the time this request will end up being sent.

                        lastRequestTimes[rateLimitingRule.Endpoint] = currentTime + delay;

                        rateLimitingRequired = true;

                    }
                    else {

                        // We can make the request immediately and don't need to wait.

                        lastRequestTimes[rateLimitingRule.Endpoint] = currentTime;

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

        private readonly ICollection<IRateLimitingRule> rules = new ConcurrentCollectionDecorator<IRateLimitingRule>(new List<IRateLimitingRule>());
        private readonly IDictionary<string, DateTimeOffset> lastRequestTimes = new Dictionary<string, DateTimeOffset>();
        private readonly object globalRequestMutex = new object();
        private static readonly Random randomSource = new Random();

        private TimeSpan GetDelay(IHttpWebRequest request, IRateLimitingRule rateLimitingRule) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            if (rateLimitingRule is null)
                throw new ArgumentNullException(nameof(rateLimitingRule));

            TimeSpan ruleDelay = TimeSpan.FromMilliseconds(Math.Ceiling(rateLimitingRule.TimePeriod.TotalMilliseconds / rateLimitingRule.RequestsPerTimePeriod));

            return new[] {
                ruleDelay,
                MaximumDelayBetweenRequests,
                TimeSpan.FromMilliseconds(request.Timeout)
            }.Min();

        }

    }

}
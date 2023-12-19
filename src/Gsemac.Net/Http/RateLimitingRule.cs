using Gsemac.Text.PatternMatching;
using System;

namespace Gsemac.Net.Http {

    public class RateLimitingRule :
        IRateLimitingRule {

        // Public members

        public IPatternMatcher Pattern { get; }
        public int RequestsPerTimePeriod { get; }
        public TimeSpan TimePeriod { get; }

        public RateLimitingRule(TimeSpan delayBetweenRequests) :
            this(requestsPerTimePeriod: 1, delayBetweenRequests) {
        }
        public RateLimitingRule(IPatternMatcher pattern, TimeSpan delayBetweenRequests) :
           this(pattern, requestsPerTimePeriod: 1, delayBetweenRequests) {
        }
        public RateLimitingRule(string pattern, TimeSpan delayBetweenRequests) :
         this(pattern, requestsPerTimePeriod: 1, delayBetweenRequests) {
        }
        public RateLimitingRule(int requestsPerTimePeriod, TimeSpan timePeriod) :
            this(PatternMatcher.Any, requestsPerTimePeriod, timePeriod) {
        }
        public RateLimitingRule(string pattern, int requestsPerTimePeriod, TimeSpan timePeriod) :
            this(new WildcardPattern(pattern), requestsPerTimePeriod, timePeriod) {
        }
        public RateLimitingRule(IPatternMatcher pattern, int requestsPerTimePeriod, TimeSpan timePeriod) {

            if (pattern is null)
                throw new ArgumentNullException(nameof(pattern));

            if (requestsPerTimePeriod <= 0)
                throw new ArgumentOutOfRangeException(nameof(requestsPerTimePeriod));

            if (timePeriod <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timePeriod));

            Pattern = pattern;
            RequestsPerTimePeriod = requestsPerTimePeriod;
            TimePeriod = timePeriod;

        }

    }

}
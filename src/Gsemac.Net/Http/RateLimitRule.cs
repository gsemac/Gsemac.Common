using Gsemac.Text.PatternMatching;
using System;

namespace Gsemac.Net.Http {

    public sealed class RateLimitRule :
        IRateLimitRule {

        // Public members

        public IPatternMatcher Pattern { get; }
        public int MaxRequests { get; }
        public TimeSpan TimeWindow { get; }

        public RateLimitRule(TimeSpan timeBetweenRequests) :
            this(maxRequests: 1, timeBetweenRequests) {
        }
        public RateLimitRule(string pattern, TimeSpan timeBetweenRequests) :
            this(pattern, maxRequests: 1, timeBetweenRequests) {
        }
        public RateLimitRule(IPatternMatcher pattern, TimeSpan delay) :
            this(pattern, maxRequests: 1, delay) {
        }
        public RateLimitRule(int maxRequests, TimeSpan timeWindow) :
            this(PatternMatcher.Any, maxRequests, timeWindow) {
        }
        public RateLimitRule(string pattern, int maxRequests, TimeSpan timeWindow) :
            this(new WildcardPattern(pattern), maxRequests, timeWindow) {
        }
        public RateLimitRule(IPatternMatcher pattern, int maxRequests, TimeSpan timeWindow) {

            if (pattern is null)
                throw new ArgumentNullException(nameof(pattern));

            if (maxRequests <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxRequests));

            if (timeWindow <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeWindow));

            Pattern = pattern;
            MaxRequests = maxRequests;
            TimeWindow = timeWindow;

        }

    }

}
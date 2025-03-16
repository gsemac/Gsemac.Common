using Gsemac.Text.PatternMatching;
using System;

namespace Gsemac.Net.Http {

    public interface IRateLimitRule {

        IPatternMatcher Pattern { get; }
        int MaxRequests { get; }
        TimeSpan TimeWindow { get; }

    }

}
using Gsemac.Text.PatternMatching;
using System;

namespace Gsemac.Net.Http {

    public interface IRateLimitingRule {

        IPatternMatcher Pattern { get; }
        int RequestsPerTimePeriod { get; }
        TimeSpan TimePeriod { get; }

    }

}
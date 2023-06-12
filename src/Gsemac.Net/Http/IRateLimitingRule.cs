using System;

namespace Gsemac.Net.Http {

    public interface IRateLimitingRule {

        string Endpoint { get; }

        int RequestsPerTimePeriod { get; }
        TimeSpan TimePeriod { get; }

    }

}
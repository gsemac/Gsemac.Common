using System;

namespace Gsemac.Net.Http {

    public class RateLimitingRule :
        IRateLimitingRule {

        // Public members

        public string Endpoint { get; }
        public int RequestsPerTimePeriod { get; }
        public TimeSpan TimePeriod { get; }

        public RateLimitingRule(int requestsPerTimePeriod, TimeSpan timePeriod) :
            this("*", requestsPerTimePeriod, timePeriod) {
        }
        public RateLimitingRule(string endpoint, int requestsPerTimePeriod, TimeSpan timePeriod) {

            if (endpoint is null)
                throw new ArgumentNullException(nameof(endpoint));

            if (requestsPerTimePeriod <= 0)
                throw new ArgumentOutOfRangeException(nameof(requestsPerTimePeriod));

            if (timePeriod <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timePeriod));

            Endpoint = endpoint;
            RequestsPerTimePeriod = requestsPerTimePeriod;
            TimePeriod = timePeriod;

        }

    }

}
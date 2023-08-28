namespace Gsemac.Net.Http {

    internal sealed class ChromeHttpHeaderComparer :
        HttpHeaderComparerBase {

        // Protected members

        protected override string[] GetHeaderOrdering() {

            // The following ordering is taken from Google Chrome (116).

            return new[] {
                "host",
                "connection",
                "pragma",
                "cache-control",
                "upgrade-insecure-requests",
                "user-agent",
                "accept",
                "accept-encoding",
                "accept-language",
            };

        }

    }

}
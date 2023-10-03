namespace Gsemac.Net.Http {

    internal sealed class ChromeHttpHeaderComparer :
        HttpHeaderComparerBase {

        // Protected members

        protected override string[] GetHeaderOrdering() {

            // The following ordering is taken from Google Chrome (117).

            return new[] {
                "host",
                "connection",
                "pragma",
                "cache-control",
                "DNT",
                "upgrade-insecure-requests",
                "user-agent",
                "accept",
                "origin",
                "referer",
                "accept-encoding",
                "accept-language",
                "cookie",
            };

        }

    }

}
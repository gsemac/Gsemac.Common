namespace Gsemac.Net.Http {

    internal sealed class FirefoxHttpHeaderComparer :
        HttpHeaderComparerBase {

        // Protected members

        protected override string[] GetHeaderOrdering() {

            // The following ordering is taken from Firefox (122).

            return new[] {
                "host",
                "user-agent",
                "accept",
                "accept-language",
                "accept-encoding",
                "referer",
                "origin",
                "dnt",
                "sec-gpc",
                "connection",
                "sec-fetch-dest",
                "sec-fetch-mode",
                "sec-fetch-site",
                "cookie",
                "upgrade-insecure-requests",
                "pragma",
                "cache-control",
            };

        }

    }

}
namespace Gsemac.Net.Http {

    internal sealed class FirefoxHttpHeaderComparer :
        HttpHeaderComparerBase {

        // Protected members

        protected override string[] GetHeaderOrdering() {

            // The following ordering is taken from Firefox (116).

            return new[] {
                "host",
                "user-agent",
                "accept",
                "accept-language",
                "accept-encoding",
                "connection",
                "upgrade-insecure-requests",
            };

        }

    }

}
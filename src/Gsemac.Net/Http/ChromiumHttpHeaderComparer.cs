using Gsemac.Net.Http.Headers;
using System;

namespace Gsemac.Net.Http {

    internal sealed class ChromiumHttpHeaderComparer :
        HttpHeaderComparerBase {

        // Protected members

        protected override int GetHeaderIndex(IHttpHeader header) {

            if (header is object && !string.IsNullOrWhiteSpace(header.Name) && header.Name.StartsWith("sec-ch-ua-", StringComparison.OrdinalIgnoreCase)) {

                // Chromium's "sec-ch-ua-*" headers are ordered randomly, but always after the "connection" header.

                return random.Next(2, GetHeaderOrdering().Length + 1);

            }

            return base.GetHeaderIndex(header);

        }
        protected override string[] GetHeaderOrdering() {

            // The following ordering is taken from Google Chrome (121).

            return new[] {
                "host",
                "connection",
                "pragma",
                "cache-control",
                "dnt",
                "upgrade-insecure-requests",
                "user-agent",
                "accept",
                "origin",
                "sec-fetch-site",
                "sec-fetch-mode",
                "sec-fetch-dest",
                "referer",
                "accept-encoding",
                "accept-language",
                "cookie",
            };

        }

        // Private members

        private readonly static Random random = new Random();

    }

}
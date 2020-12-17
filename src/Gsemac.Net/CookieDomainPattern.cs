using System;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class CookieDomainPattern {

        // Public members

        public CookieDomainPattern(string domain) {

            domain = domain.TrimStart('.').TrimEnd('/');

            // Domain cannot be a lone TLD.

            if (!domain.Contains("."))
                throw new ArgumentException("The given pattern is invalid.", nameof(domain));

            this.domain = domain;

        }

        public bool IsMatch(string uri) {

            uri = uri.TrimStart('.').TrimEnd('/');

            // Follow the comparison rules as described here: https://stackoverflow.com/a/1063760

            Regex urlPartsPattern = new Regex(@"^(?:[^:]+:\/\/)?([^\/]+)(.*$)");

            Match urlPartsMatch = urlPartsPattern.Match(uri);
            Match cookieDomainPartsMatch = urlPartsPattern.Match(domain);

            string hostname = urlPartsMatch.Groups[1].Value;
            string path = urlPartsMatch.Groups[2].Value;
            string cookieDomainHostname = cookieDomainPartsMatch.Groups[1].Value;
            string cookieDomainPath = cookieDomainPartsMatch.Groups[2].Value;

            // Note the domain should be case-insensitive, but the path should be case-sensitive: https://stackoverflow.com/a/400013

            return (hostname.Equals(cookieDomainHostname, StringComparison.OrdinalIgnoreCase) || hostname.EndsWith("." + cookieDomainHostname, StringComparison.OrdinalIgnoreCase)) &&
                path.Equals(cookieDomainPath);

        }
        public bool IsMatch(Uri uri) {

            return IsMatch(uri.AbsoluteUri);

        }

        // Private members

        private readonly string domain;

    }

}
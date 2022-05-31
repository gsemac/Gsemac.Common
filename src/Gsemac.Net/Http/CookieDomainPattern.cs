using Gsemac.IO;
using System;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Http {

    public class CookieDomainPattern {

        // Public members

        public CookieDomainPattern(string domain) {

            domain = domain.TrimStart('.').TrimEnd('/');

            // Domain cannot be a lone TLD.

            if (!domain.Contains("."))
                throw new ArgumentException(Properties.ExceptionMessages.InvalidCookieDomainPattern, nameof(domain));

            this.domain = domain;

        }

        public bool IsMatch(string uri) {

            uri = Url.StripFragment(Url.StripQueryParameters(uri.TrimStart('.').TrimEnd('/')));

            // Follow the comparison rules as described here:
            // https://stackoverflow.com/a/1063760

            Regex urlPartsPattern = new Regex(@"^(?:[^:]+:\/\/)?(?<hostname>[^\/]+)(?<path>.*$)");

            Match urlPartsMatch = urlPartsPattern.Match(uri);
            Match cookieDomainPartsMatch = urlPartsPattern.Match(domain);

            string hostname = urlPartsMatch.Groups["hostname"].Value;
            string path = urlPartsMatch.Groups["path"].Value;
            string cookieDomainHostname = cookieDomainPartsMatch.Groups["hostname"].Value;
            string cookieDomainPath = cookieDomainPartsMatch.Groups["path"].Value;

            // Note the domain should be case-insensitive, but the path should be case-sensitive:
            // https://stackoverflow.com/a/400013

            // Subpaths should have access to cookies set for their superpaths, but NOT the other way around:
            // https://stackoverflow.com/a/576561/5383169

            bool hostnameMatches = hostname.Equals(cookieDomainHostname, StringComparison.OrdinalIgnoreCase) || hostname.EndsWith("." + cookieDomainHostname, StringComparison.OrdinalIgnoreCase);
            bool pathMatches = string.IsNullOrEmpty(cookieDomainPath) || path.Equals(cookieDomainPath) || PathUtilities.IsSubpathOf(cookieDomainPath, path);

            return hostnameMatches && pathMatches;

        }
        public bool IsMatch(Uri uri) {

            return IsMatch(uri.AbsoluteUri);

        }

        // Private members

        private readonly string domain;

    }

}
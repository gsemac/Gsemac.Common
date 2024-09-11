using Gsemac.Core;
using System;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.Http {

    public sealed class CookieComparer :
        IComparer<Cookie>,
        IEqualityComparer<Cookie> {

        public int Compare(Cookie x, Cookie y) {

            // Cookies are sorted by domain > name > path > timestamp.

            int nullityComparisonResult = CompareNullity(x, y);

            if (nullityComparisonResult != 0)
                return nullityComparisonResult;

            int domainComparisonResult = CompareDomains(x, y);

            if (domainComparisonResult != 0)
                return domainComparisonResult;

            int nameComparisonResult = CompareNames(x, y);

            if (nameComparisonResult != 0)
                return nameComparisonResult;

            int pathComparisonResult = ComparePaths(x, y);

            if (pathComparisonResult != 0)
                return pathComparisonResult;

            return x.TimeStamp.CompareTo(y.TimeStamp);

        }
        public bool Equals(Cookie x, Cookie y) {
            return GetHashCode(x).Equals(GetHashCode(y));
        }

        public int GetHashCode(Cookie obj) {

            // Two cookies are considered equal if they have the same name, domain, and path.

            if (obj is null)
                return EqualityComparer<Cookie>.Default.GetHashCode(obj);

            return new HashCodeBuilder()
                .WithValue(StringComparer.OrdinalIgnoreCase.GetHashCode(NormalizeDomain(obj.Domain)))
                .WithValue(StringComparer.Ordinal.GetHashCode(obj.Name))
                .WithValue(StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Path))
                .GetHashCode();

        }

        // Private members

        private static string NormalizeDomain(string domain) {
            return HttpUtilities.NormalizeCookieDomain(domain);
        }
        private static int CompareNullity(Cookie x, Cookie y) {

            if (x is null && y is null)
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            return 0;

        }
        private static int CompareNames(Cookie x, Cookie y) {

            // Cookie names are case-sensitive.

            string nameX = x?.Name ?? string.Empty;
            string nameY = y?.Name ?? string.Empty;

            return StringComparer.Ordinal.Compare(nameX, nameY);

        }
        private static int CompareDomains(Cookie x, Cookie y) {

            // Per RFC 6265, the leading dot is ignored when comparing domains.
            // Domain comparisons are also not case-sensitive.

            string domainX = NormalizeDomain(x.Domain);
            string domainY = NormalizeDomain(y.Domain);

            return StringComparer.OrdinalIgnoreCase.Compare(domainX, domainY);

        }
        private static int ComparePaths(Cookie x, Cookie y) {

            // Paths are case-sensitive.
            // Per RFC 6265, longer paths SHOULD be ordered before shorter paths.

            string pathX = x?.Path ?? string.Empty;
            string pathY = y?.Path ?? string.Empty;

            if (pathX.Length > pathY.Length)
                return -1;
            else if (pathX.Length < pathY.Length)
                return 1;

            return StringComparer.Ordinal.Compare(pathX, pathY);

        }

    }

}
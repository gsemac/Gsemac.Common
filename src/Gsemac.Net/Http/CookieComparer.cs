using System;
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.Http {

    public sealed class CookieComparer :
        IComparer<Cookie>,
        IEqualityComparer<Cookie> {

        public int Compare(Cookie x, Cookie y) {

            int nullityComparisonResult = CompareNullity(x, y);

            if (nullityComparisonResult != 0)
                return nullityComparisonResult;

            int nameComparisonResult = CompareNames(x, y);

            if (nameComparisonResult != 0)
                return nameComparisonResult;

            int domainComparisonResult = CompareDomains(x, y);

            if (domainComparisonResult != 0)
                return domainComparisonResult;

            int pathComparisonResult = ComparePaths(x, y);

            if (pathComparisonResult != 0)
                return pathComparisonResult;

            return 0;

        }
        public bool Equals(Cookie x, Cookie y) {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(Cookie obj) {
            return EqualityComparer<Cookie>.Default.GetHashCode(obj);
        }

        // Private members

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

            string domainX = x?.Domain?.TrimStart('.') ?? string.Empty;
            string domainY = y?.Domain?.TrimStart('.') ?? string.Empty;

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
using Gsemac.Core.Properties;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Core {

    public static class Version {

        // Public members

        public static bool TryParse(string input, out IVersion result) {

            return TryParse(input, strict: true, out result);

        }
        public static bool TryParse(string input, bool strict, out IVersion result) {

            result = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Strip any extraneous text from the version string.
            // Allow inputs like "version 1.0" and "v1.0".

            input = Regex.Replace(input, @"^v(?:ersion)?\s*", string.Empty, RegexOptions.IgnoreCase);

            if (MSVersion.TryParse(input, strict, out MSVersion msVersion))
                result = msVersion;
            else if (SemVersion.TryParse(input, strict, out SemVersion semVersion))
                result = semVersion;

            return result != null;

        }
        public static IVersion Parse(string input) {

            return Parse(input, strict: true);

        }
        public static IVersion Parse(string input, bool strict) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, strict, out IVersion result))
                throw new FormatException(string.Format(ExceptionMessages.InvalidVersionString, input));

            return result;

        }

        internal static int Compare(object lhs, object rhs) {

            if (lhs is null)
                throw new ArgumentNullException(nameof(lhs));

            if (rhs is null)
                throw new ArgumentNullException(nameof(rhs));

            // If strings were given, attempt to parse them as versions.

            if (lhs is string lhsString && TryParse(lhsString, out IVersion lhsParsedVersion))
                lhs = lhsParsedVersion;

            if (rhs is string rhsString && TryParse(rhsString, out IVersion rhsParsedVersion))
                rhs = rhsParsedVersion;

            if (lhs is IVersion lhsVersion && rhs is IVersion rhsVersion)
                return Compare(lhsVersion, rhsVersion);

            // The inputs were not valid.

            throw new ArgumentException("One or both of the given objects are not valid version objects.");

        }
        internal static int Compare(IVersion lhs, IVersion rhs) {

            if (lhs is null)
                throw new ArgumentNullException(nameof(lhs));

            if (rhs is null)
                throw new ArgumentNullException(nameof(rhs));

            // If we can determine the version type, perform version-specific comparisons.

            if (lhs is MSVersion lhsMsVersion && rhs is MSVersion rhsMsVersion)
                return lhsMsVersion.CompareTo(rhsMsVersion);

            if (lhs is SemVersion lhsSemVersion && rhs is SemVersion rhsSemVersion)
                return lhsSemVersion.CompareTo(rhsSemVersion);

            // Otherwise, perform a generic comparison.

            return Compare(lhs.RevisionNumbers.ToArray(), rhs.RevisionNumbers.ToArray());

        }
        internal static int Compare(int[] lhs, int[] rhs) {

            // Compare revision numbers in order.
            // This works for any number of revision numbers.

            int length = Math.Max(lhs.Length, rhs.Length);

            int[] lhsRevisionNumbers = new int[length];
            int[] rhsRevisionNumbers = new int[length];

            Array.Copy(lhs, lhsRevisionNumbers, lhs.Length);
            Array.Copy(rhs, rhsRevisionNumbers, rhs.Length);

            for (int i = 0; i < length; ++i) {

                if (lhsRevisionNumbers[i] > rhsRevisionNumbers[i])
                    return 1;

                if (lhsRevisionNumbers[i] < rhsRevisionNumbers[i])
                    return -1;

            }

            return 0;

        }

    }

}
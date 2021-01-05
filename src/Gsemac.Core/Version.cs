﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Core {

    public static class Version {

        public static bool TryParse(string input, out IVersion result) {

            result = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Strip any extraneous text from the version string.
            // Allow inputs like "version 1.0" and "v1.0".

            input = Regex.Replace(input, @"^v(?:ersion)?\s*", string.Empty, RegexOptions.IgnoreCase);

            if (MSVersion.TryParse(input, out MSVersion msVersion))
                result = msVersion;
            else if (SemVersion.TryParse(input, out SemVersion semVersion))
                result = semVersion;

            return result != null;

        }
        public static IVersion Parse(string input) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, out IVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

        public static int Compare(object lhs, object rhs) {

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
        public static int Compare(IVersion lhs, IVersion rhs) {

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

            int length = Math.Max(lhs.Count(), rhs.Count());

            int[] lhsRevisionNumbers = new int[length];
            int[] rhsRevisionNumbers = new int[length];

            Array.Copy(lhs.ToArray(), lhsRevisionNumbers, length);
            Array.Copy(rhs.ToArray(), rhsRevisionNumbers, length);

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
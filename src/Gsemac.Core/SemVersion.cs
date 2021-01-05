using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gsemac.Core {

    // Documentation for SemVer: https://semver.org/

    public class SemVersion :
        IVersion,
        IComparable<SemVersion> {

        // Public members

        public bool IsPreRelease => !string.IsNullOrEmpty(PreRelease);

        public int Major => revisionNumbers[0];
        public int Minor => revisionNumbers.Length > 1 ? revisionNumbers[1] : 0;
        public int Patch => revisionNumbers.Length > 2 ? revisionNumbers[2] : 0;
        public string PreRelease { get; private set; } = string.Empty;
        public string Build { get; private set; } = string.Empty;

        public SemVersion(int major, int minor, int patch) {

            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major));

            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor));

            if (patch < 0)
                throw new ArgumentOutOfRangeException(nameof(patch));

            this.revisionNumbers = new int[] {
                major,
                minor,
                patch
            };

        }
        public SemVersion(int major, int minor, int patch, string preRelease) :
            this(major, minor, patch) {

            if (string.IsNullOrEmpty(preRelease))
                throw new ArgumentNullException(nameof(preRelease));

            if (string.IsNullOrWhiteSpace(preRelease))
                throw new ArgumentException("Pre-release string cannot be empty.", nameof(preRelease));

            this.PreRelease = preRelease;

        }
        public SemVersion(int major, int minor, int patch, string preRelease, string build) :
            this(major, minor, patch, preRelease) {

            if (string.IsNullOrEmpty(build))
                throw new ArgumentNullException(nameof(build));

            if (string.IsNullOrWhiteSpace(build))
                throw new ArgumentException("Build string cannot be empty.", nameof(build));

            this.Build = build;

        }
        public SemVersion(string versionString) :
            this(Parse(versionString)) {
        }
        public SemVersion(SemVersion other) :
            this(other.revisionNumbers) {

            PreRelease = other.PreRelease;
            Build = other.Build;

        }

        public int CompareTo(object obj) {

            return Version.Compare(this, obj);

        }
        public int CompareTo(IVersion other) {

            return Version.Compare(this, other);

        }
        public int CompareTo(SemVersion other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            // Compare revision numbers.

            int revisionNumbersComparisonResult = Version.Compare(this.revisionNumbers, other.revisionNumbers);

            if (revisionNumbersComparisonResult != 0)
                return revisionNumbersComparisonResult;

            // If we get here, the major/minor/patch are all equal.

            if (IsPreRelease && !other.IsPreRelease)
                return -1;

            if (!IsPreRelease && other.IsPreRelease)
                return 1;

            // If we get here, both versions are pre-release versions.

            string[] preReleaseParts = PreRelease?.Split('.') ?? new string[] { };
            string[] otherPreReleaseParts = other.PreRelease?.Split('.') ?? new string[] { };

            for (int i = 0; i < preReleaseParts.Count() && i < otherPreReleaseParts.Count(); ++i) {

                int compareResult = ComparePreReleaseIdentifiers(preReleaseParts[i], otherPreReleaseParts[i]);

                if (compareResult != 0)
                    return compareResult;

            }

            // If we get here, all pre-release identifiers were identical.

            if (preReleaseParts.Count() > otherPreReleaseParts.Count())
                return 1;
            else if (preReleaseParts.Count() < otherPreReleaseParts.Count())
                return -1;

            // If we get here, the versions were exactly equal.

            return 0;

        }

        public override bool Equals(object obj) {

            if (ReferenceEquals(this, obj))
                return true;

            if (ReferenceEquals(obj, null))
                return false;

            return CompareTo(obj) == 0;
        }
        public override int GetHashCode() {

            return ToString().GetHashCode();

        }

        public static bool operator ==(SemVersion left, SemVersion right) {

            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }
        public static bool operator !=(SemVersion left, SemVersion right) {

            return !(left == right);

        }
        public static bool operator <(SemVersion left, SemVersion right) {

            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        }
        public static bool operator <=(SemVersion left, SemVersion right) {

            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        }
        public static bool operator >(SemVersion left, SemVersion right) {

            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        }
        public static bool operator >=(SemVersion left, SemVersion right) {

            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;

        }

        public IEnumerator<int> GetEnumerator() {

            return ((IEnumerable<int>)revisionNumbers).GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(string.Join(".", revisionNumbers));

            if (!string.IsNullOrEmpty(PreRelease)) {

                sb.Append("-");
                sb.Append(PreRelease);

            }

            if (!string.IsNullOrEmpty(Build)) {

                sb.Append("+");
                sb.Append(Build);

            }

            return sb.ToString();

        }

        public static bool TryParse(string input, out SemVersion result) {

            return TryParse(input, strict: true, out result);

        }
        public static bool TryParse(string input, bool strict, out SemVersion result) {

            result = null;

            // The input string must not be empty.

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Split the version into its numeric (e.g. "1.0.0") and pre-release/build (e.g. "alpha+build") parts.

            string[] parts = input.Split(new char[] { '-' }, 2);

            // The version string must at least have the numeric part.

            if (!parts.Any())
                return false;

            int?[] revisionNumbers = parts.First().Split('.')
                .Select(part => int.TryParse(part, out int parsedInt) ? (int?)parsedInt : null)
                .ToArray();

            // The numeric part must have exactly 3 parts, and they must all be > 0.
            // If strict is false, we will allow any number of revision numbers (at least one).

            if (!revisionNumbers.Any() || revisionNumbers.Any(part => !part.HasValue || part.Value < 0))
                return false;

            if (strict && revisionNumbers.Count() != 3)
                return false;

            result = new SemVersion(revisionNumbers.Select(i => i.Value).ToArray());

            if (parts.Count() > 1) {

                string[] preReleaseAndBuild = parts.Last().Split(new char[] { '+' }, 2);

                // The pre-release string must not be empty.

                if (string.IsNullOrWhiteSpace(preReleaseAndBuild[0]))
                    return false;

                result.PreRelease = preReleaseAndBuild[0];

                if (preReleaseAndBuild.Count() == 2) {

                    // The build metadata string must not be empty.

                    if (string.IsNullOrWhiteSpace(preReleaseAndBuild[1]))
                        return false;

                    result.Build = preReleaseAndBuild[1];

                }

            }

            return true;

        }
        public static SemVersion Parse(string input) {

            return Parse(input, strict: true);

        }
        public static SemVersion Parse(string input, bool strict) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, strict: strict, out SemVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

        // Private members

        private readonly int[] revisionNumbers;

        private SemVersion(int[] revisionNumbers) {

            this.revisionNumbers = revisionNumbers;

        }

        private static int ComparePreReleaseIdentifiers(string lhs, string rhs) {

            if (int.TryParse(lhs, NumberStyles.Integer, CultureInfo.InvariantCulture, out int lhsInt) && int.TryParse(rhs, NumberStyles.Integer, CultureInfo.InvariantCulture, out int rhsInt))
                return lhsInt.CompareTo(rhsInt);

            return lhs.CompareTo(rhs);

        }

    }

}
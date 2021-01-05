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

        public int Major { get; } = 0;
        public int Minor { get; } = 0;
        public int Patch { get; } = 0;
        public string PreRelease { get; private set; } = string.Empty;
        public string Build { get; private set; } = string.Empty;

        public SemVersion(int major, int minor) :
            this(major, minor, 0) {
        }
        public SemVersion(int major, int minor, int patch) {

            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major));

            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor));

            if (patch < 0)
                throw new ArgumentOutOfRangeException(nameof(patch));

            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;

        }
        public SemVersion(string versionString) {

            SemVersion version = Parse(versionString);

            Major = version.Major;
            Minor = version.Minor;
            Build = version.Build;
            Patch = version.Patch;
            PreRelease = version.PreRelease;
            Build = version.Build;

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

            if (Major < other.Major || Minor < other.Minor || Patch < other.Patch)
                return -1;
            else if (Major > other.Major || Minor > other.Minor || Patch > other.Patch)
                return 1;

            // If we get here, the major/minor/patch are all equal.

            if (IsPreRelease && !other.IsPreRelease)
                return -1;

            if (!IsPreRelease && other.IsPreRelease)
                return 1;

            // If we get here, both versions are pre-release versions.

            string[] preReleaseParts = PreRelease?.Split('.') ?? new string[] { };
            string[] objPreReleaseParts = other.PreRelease?.Split('.') ?? new string[] { };

            for (int i = 0; i < preReleaseParts.Count() && i < objPreReleaseParts.Count(); ++i) {

                int compareResult = ComparePreReleaseIdentifiers(preReleaseParts[i], objPreReleaseParts[i]);

                if (compareResult != 0)
                    return compareResult;

            }

            // If we get here, all pre-release identifiers were identical.

            if (preReleaseParts.Count() > objPreReleaseParts.Count())
                return 1;
            else if (preReleaseParts.Count() < objPreReleaseParts.Count())
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

            yield return Major;
            yield return Minor;
            yield return Patch;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Major);
            sb.Append('.');
            sb.Append(Minor);
            sb.Append('.');
            sb.Append(Patch);

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

            result = null;

            // The input string must not be empty.

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Split the version into its numeric (e.g. "1.0.0") and pre-release/build (e.g. "alpha+build") parts.

            string[] parts = input.Split(new char[] { '-' }, 2);

            // The version string must at least have the numeric part.

            if (!parts.Any())
                return false;

            int?[] numericParts = parts.First().Split('.')
                .Select(part => int.TryParse(part, out int parsedInt) ? (int?)parsedInt : null)
                .ToArray();

            // The numeric part must have exactly 3 parts, and they must all be > 0.

            if (numericParts.Count() != 3 || numericParts.Any(part => !part.HasValue || part.Value < 0))
                return false;

            result = new SemVersion(numericParts[0].Value, numericParts[1].Value, numericParts[2].Value);

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

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, out SemVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

        // Private members

        private static int ComparePreReleaseIdentifiers(string lhs, string rhs) {

            if (int.TryParse(lhs, NumberStyles.Integer, CultureInfo.InvariantCulture, out int lhsInt) && int.TryParse(rhs, NumberStyles.Integer, CultureInfo.InvariantCulture, out int rhsInt))
                return lhsInt.CompareTo(rhsInt);

            return lhs.CompareTo(rhs);

        }

    }

}
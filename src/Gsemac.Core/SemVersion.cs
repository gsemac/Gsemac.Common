using System;
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

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is SemVersion semVersion)
                return CompareTo(semVersion);
            else if (obj is IVersion version)
                return CompareTo(version);
            else if (obj is string versionString && Version.TryParse(versionString, out IVersion parsedVersion))
                return CompareTo(parsedVersion);
            else
                throw new ArgumentException("The given object is not a valid version.", nameof(obj));

        }
        public int CompareTo(SemVersion obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (Major < obj.Major || Minor < obj.Minor || Patch < obj.Patch)
                return -1;
            else if (Major > obj.Major || Minor > obj.Minor || Patch > obj.Patch)
                return 1;

            // If we get here, the major/minor/patch are all equal.

            if (IsPreRelease && !obj.IsPreRelease)
                return -1;

            if (!IsPreRelease && obj.IsPreRelease)
                return 1;

            // If we get here, both versions are pre-release versions.

            string[] preReleaseParts = PreRelease?.Split('.') ?? new string[] { };
            string[] objPreReleaseParts = obj.PreRelease?.Split('.') ?? new string[] { };

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
        public int CompareTo(IVersion obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is SemVersion semVersion)
                return CompareTo(semVersion);

            if (Major < obj.Major || Minor < obj.Minor)
                return -1;
            else if (Major > obj.Major || Minor > obj.Minor || (!IsPreRelease && obj.IsPreRelease))
                return 1;
            else
                return 0;

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
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gsemac.Core {

    // Documentation for Microsoft's version format:
    // https://docs.microsoft.com/en-us/previous-versions/dotnet/articles/ms973869(v=msdn.10)?redirectedfrom=MSDN

    public class MSVersion :
        IVersion,
        IComparable<MSVersion> {

        // Public members

        public bool IsPreRelease => false;

        public int Major { get; } = -1;
        public int Minor { get; } = -1;
        public int Build { get; } = -1;
        public int Revision { get; } = -1;

        public MSVersion(int major, int minor) {

            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major));

            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor));

            this.Major = major;
            this.Minor = minor;

        }
        public MSVersion(int major, int minor, int build) :
            this(major, minor) {

            if (build < 0)
                throw new ArgumentOutOfRangeException(nameof(build));

            this.Build = build;

        }
        public MSVersion(int major, int minor, int build, int revision) :
            this(major, minor, build) {

            if (revision < 0)
                throw new ArgumentOutOfRangeException(nameof(revision));

            this.Revision = revision;

        }
        public MSVersion(string versionString) {

            MSVersion version = Parse(versionString);

            Major = version.Major;
            Minor = version.Minor;
            Build = version.Build;
            Revision = version.Revision;

        }

        public int CompareTo(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is MSVersion msVersion)
                return CompareTo(msVersion);
            else if (obj is IVersion version)
                return CompareTo(version);
            else if (obj is string versionString && Version.TryParse(versionString, out IVersion parsedVersion))
                return CompareTo(parsedVersion);
            else
                throw new ArgumentException("The given object is not a valid version.", nameof(obj));

        }
        public int CompareTo(MSVersion obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (Major < obj.Major || Minor < obj.Minor || Build < obj.Build || Revision < obj.Revision)
                return -1;
            else if (Major > obj.Major || Minor > obj.Minor || Build > obj.Build || Revision > obj.Revision)
                return 1;
            else
                return 0;

        }
        public int CompareTo(IVersion obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is MSVersion msVersion)
                return CompareTo(msVersion);

            if (Major < obj.Major || Minor < obj.Minor)
                return -1;
            else if (obj.IsPreRelease || Major > obj.Major || Minor > obj.Minor)
                return 1;
            else
                return 0;

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Major);
            sb.Append('.');
            sb.Append(Minor);

            if (Build >= 0) {

                sb.Append('.');
                sb.Append(Build);

            }

            if (Revision >= 0) {

                sb.Append('.');
                sb.Append(Revision);

            }

            return sb.ToString();

        }

        public static bool TryParse(string input, out MSVersion result) {

            result = null;

            // The input string must not be empty.

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Version strings are of the form major.minor[.build[.revision]].

            int?[] parts = input.Split('.')
                .Select(part => int.TryParse(part, out int parsedInt) ? (int?)parsedInt : null)
                .ToArray();

            // There must be at least two parts, they must all be numeric, and they must all be > 0.

            if (parts.Count() < 2 || parts.Any(part => !part.HasValue || part.Value < 0))
                return false;

            if (parts.Count() == 4)
                result = new MSVersion(parts[0].Value, parts[1].Value, parts[2].Value, parts[3].Value);
            else if (parts.Count() == 3)
                result = new MSVersion(parts[0].Value, parts[1].Value, parts[2].Value);
            else
                result = new MSVersion(parts[0].Value, parts[1].Value);

            return true;

        }
        public static MSVersion Parse(string input) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, out MSVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

    }

}
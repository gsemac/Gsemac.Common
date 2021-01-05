using System;
using System.Collections;
using System.Collections.Generic;
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
        public int Build => build < 0 ? 0 : build;
        public int Revision => revision < 0 ? 0 : revision;

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

            this.build = build;

        }
        public MSVersion(int major, int minor, int build, int revision) :
            this(major, minor, build) {

            if (revision < 0)
                throw new ArgumentOutOfRangeException(nameof(revision));

            this.revision = revision;

        }
        public MSVersion(string versionString) {

            MSVersion version = Parse(versionString);

            this.Major = version.Major;
            this.Minor = version.Minor;
            this.build = version.build;
            this.revision = version.revision;

        }

        public int CompareTo(object obj) {

            return Version.Compare(this, obj);

        }
        public int CompareTo(IVersion other) {

            return Version.Compare(this, other);

        }
        public int CompareTo(MSVersion other) {

            int[] lhsRevisionNumbers = new int[] { Major, Minor, Build, Revision };
            int[] rhsRevisionNumbers = new int[] { other.Major, other.Minor, other.Build, other.Revision };

            for (int i = 0; i < Math.Min(lhsRevisionNumbers.Length, rhsRevisionNumbers.Length); ++i) {

                if (lhsRevisionNumbers[i] > rhsRevisionNumbers[i])
                    return 1;

                if (lhsRevisionNumbers[i] < rhsRevisionNumbers[i])
                    return -1;

            }

            return 0;

        }

        public override bool Equals(object obj) {

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null)
                return false;

            return CompareTo(obj) == 0;

        }
        public override int GetHashCode() {

            return ToString().GetHashCode();

        }

        public static bool operator ==(MSVersion left, MSVersion right) {

            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);

        }
        public static bool operator !=(MSVersion left, MSVersion right) {

            return !(left == right);

        }
        public static bool operator <(MSVersion left, MSVersion right) {

            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        }
        public static bool operator <=(MSVersion left, MSVersion right) {

            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        }
        public static bool operator >(MSVersion left, MSVersion right) {

            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        }
        public static bool operator >=(MSVersion left, MSVersion right) {

            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;

        }

        public IEnumerator<int> GetEnumerator() {

            yield return Major;
            yield return Minor;

            if (build >= 0)
                yield return Build;

            if (revision >= 0)
                yield return Revision;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Major);
            sb.Append('.');
            sb.Append(Minor);

            if (build >= 0) {

                sb.Append('.');
                sb.Append(Build);

            }

            if (revision >= 0) {

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

            if (parts.Count() < 2 || parts.Count() > 4 || parts.Any(part => !part.HasValue || part.Value < 0))
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

        // Private members

        private readonly int build = -1;
        private readonly int revision = -1;

    }

}
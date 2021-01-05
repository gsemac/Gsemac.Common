using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Core {

    // Documentation for Microsoft's version format:
    // https://docs.microsoft.com/en-us/previous-versions/dotnet/articles/ms973869(v=msdn.10)?redirectedfrom=MSDN

    public class MSVersion :
        IVersion,
        IComparable<MSVersion> {

        // Public members

        public bool IsPreRelease => false;

        public int Major => revisionNumbers[0];
        public int Minor => revisionNumbers.Length > 1 ? revisionNumbers[1] : 0;
        public int Build => revisionNumbers.Length > 2 ? revisionNumbers[2] : 0;
        public int Revision => revisionNumbers.Length > 3 ? revisionNumbers[3] : 0;

        public MSVersion(int major, int minor) {

            if (major < 0)
                throw new ArgumentOutOfRangeException(nameof(major));

            if (minor < 0)
                throw new ArgumentOutOfRangeException(nameof(minor));

            this.revisionNumbers = new int[] {
                major,
                minor,
            };

        }
        public MSVersion(int major, int minor, int build) :
            this(major, minor) {

            if (build < 0)
                throw new ArgumentOutOfRangeException(nameof(build));

            this.revisionNumbers = this.revisionNumbers.Concat(new int[] {
                build
            }).ToArray();

        }
        public MSVersion(int major, int minor, int build, int revision) :
            this(major, minor, build) {

            if (revision < 0)
                throw new ArgumentOutOfRangeException(nameof(revision));

            this.revisionNumbers = this.revisionNumbers.Concat(new int[] {
                revision
            }).ToArray();

        }
        public MSVersion(string versionString) {

            MSVersion version = Parse(versionString);

            this.revisionNumbers = version.revisionNumbers;

        }
        public MSVersion(IVersion other) :
            this(other.ToArray()) {
        }
        public MSVersion(System.Version other) :
            this(SystemVersionToMSVersion(other)) {
        }

        public int CompareTo(object obj) {

            return Version.Compare(this, obj);

        }
        public int CompareTo(IVersion other) {

            return Version.Compare(this, other);

        }
        public int CompareTo(MSVersion other) {

            return Version.Compare(this.revisionNumbers, other.revisionNumbers);

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

            if (left is null)
                return right is null;

            return left.Equals(right);

        }
        public static bool operator !=(MSVersion left, MSVersion right) {

            return !(left == right);

        }
        public static bool operator <(MSVersion left, MSVersion right) {

            return left is null ? right is object : left.CompareTo(right) < 0;

        }
        public static bool operator <=(MSVersion left, MSVersion right) {

            return left is null || left.CompareTo(right) <= 0;

        }
        public static bool operator >(MSVersion left, MSVersion right) {

            return left is object && left.CompareTo(right) > 0;

        }
        public static bool operator >=(MSVersion left, MSVersion right) {

            return left is null ? right is null : left.CompareTo(right) >= 0;

        }

        public IEnumerator<int> GetEnumerator() {

            return ((IEnumerable<int>)revisionNumbers).GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        public override string ToString() {

            return string.Join(".", revisionNumbers);

        }

        public static bool TryParse(string input, out MSVersion result) {

            return TryParse(input, strict: true, out result);

        }
        public static bool TryParse(string input, bool strict, out MSVersion result) {

            result = null;

            // The input string must not be empty.

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Version strings are of the form major.minor[.build[.revision]].

            int?[] revisionNumbers = input.Split('.')
                .Select(part => int.TryParse(part, out int parsedInt) ? (int?)parsedInt : null)
                .ToArray();

            // There must be at least two revision numbers, they must all be numeric, and they must all be > 0.
            // If strict is false, we will allow any number of revision numbers (at least one).

            if (!revisionNumbers.Any() || revisionNumbers.Any(part => !part.HasValue || part.Value < 0))
                return false;

            if (strict && (revisionNumbers.Count() < 2 || revisionNumbers.Count() > 4 || revisionNumbers.Any(part => !part.HasValue || part.Value < 0)))
                return false;

            result = new MSVersion(revisionNumbers.Select(i => i.Value).ToArray());

            return true;

        }
        public static MSVersion Parse(string input) {

            return Parse(input, strict: true);

        }
        public static MSVersion Parse(string input, bool strict) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (!TryParse(input, strict, out MSVersion result))
                throw new FormatException("The version string was not in the correct format.");

            return result;

        }

        // Private members

        private readonly int[] revisionNumbers;

        private MSVersion(int[] revisionNumbers) {

            this.revisionNumbers = revisionNumbers;

        }

        private static MSVersion SystemVersionToMSVersion(System.Version systemVersion) {

            if (systemVersion.Build >= 0)
                if (systemVersion.Revision >= 0)
                    return new MSVersion(systemVersion.Major, systemVersion.Minor, systemVersion.Build, systemVersion.Revision);
                else
                    return new MSVersion(systemVersion.Major, systemVersion.Minor, systemVersion.Build);

            return new MSVersion(systemVersion.Major, systemVersion.Minor);

        }

    }

}
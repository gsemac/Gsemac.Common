using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public abstract class FileFormatBase :
        IFileFormat {

        // Public members

        public abstract IEnumerable<string> Extensions { get; }
        public abstract IEnumerable<IFileSignature> Signatures { get; }
        public abstract string MimeType { get; }

        public int CompareTo(object obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is IFileFormat fileFormat)
                return CompareTo(fileFormat);

            throw new ArgumentException("The given object is not a valid file format.", nameof(obj));

        }
        public int CompareTo(IFileFormat other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Equals(other))
                return 0;

            return (MimeType ?? "").ToLowerInvariant().CompareTo((other.MimeType ?? "").ToLowerInvariant());

        }
        public override bool Equals(object obj) {

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null)
                return false;

            // We check for file extension similarities instead of comparing the mimetype since unidentified but different formats will have the same mimetype (application/octet-stream).

            if (obj is IFileFormat fileFormat)
                return Extensions.Any(ext => fileFormat.Extensions.Any(otherExt => otherExt.Equals(ext, StringComparison.OrdinalIgnoreCase)));

            throw new ArgumentException("The given object is not a valid file format.", nameof(obj));

        }
        public override int GetHashCode() {

            return (MimeType.ToLowerInvariant() ?? Extensions?.FirstOrDefault()?.ToLowerInvariant()).GetHashCode();

        }

        public override string ToString() {

            if (string.IsNullOrWhiteSpace(MimeType))
                return "application/octet-stream";

            return MimeType;

        }

        public static bool operator ==(FileFormatBase left, FileFormatBase right) {

            if (left is null)
                return right is null;

            return left.Equals(right);

        }
        public static bool operator !=(FileFormatBase left, FileFormatBase right) {

            return !(left == right);

        }
        public static bool operator <(FileFormatBase left, FileFormatBase right) {

            return left is null ? right is object : left.CompareTo(right) < 0;

        }
        public static bool operator <=(FileFormatBase left, FileFormatBase right) {

            return left is null || left.CompareTo(right) <= 0;

        }
        public static bool operator >(FileFormatBase left, FileFormatBase right) {

            return left is object && left.CompareTo(right) > 0;

        }
        public static bool operator >=(FileFormatBase left, FileFormatBase right) {

            return left is null ? right is null : left.CompareTo(right) >= 0;

        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public abstract class FileFormatBase :
        IFileFormat {

        // Public members

        public abstract IEnumerable<string> Extensions { get; }
        public virtual IEnumerable<IFileSignature> Signatures => Enumerable.Empty<IFileSignature>();
        public abstract IEnumerable<IMimeType> MimeTypes { get; }
        public abstract string Name { get; }

        public int CompareTo(object other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (other is IFileFormat fileFormat)
                return CompareTo(fileFormat);

            throw new ArgumentException(string.Format(Core.Properties.ExceptionMessages.ObjectIsNotAnInstanceOfWithType, nameof(IFileFormat)), nameof(other));

        }
        public virtual int CompareTo(IFileFormat other) {

            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (Equals(other))
                return 0;

            string lhsMimeType = GetMimeTypeString(this);
            string rhsMimeType = GetMimeTypeString(other);

            return lhsMimeType.CompareTo(rhsMimeType);

        }
        public override bool Equals(object other) {

            if (ReferenceEquals(this, other))
                return true;

            if (other is null)
                return false;

            if (other is IFileFormat fileFormat)
                return Equals(fileFormat);

            throw new ArgumentException(string.Format(Core.Properties.ExceptionMessages.ObjectIsNotAnInstanceOfWithType, nameof(IFileFormat)), nameof(other));

        }
        public virtual bool Equals(IFileFormat other) {

            if (other is null)
                return false;

            // Start by comparing the MIME types, as long as they're not generic (application/octet-stream).

            string lhsMimeType = GetMimeTypeString(this);
            string rhsMimeType = GetMimeTypeString(other);

            if (!string.IsNullOrWhiteSpace(lhsMimeType) && !lhsMimeType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
                return lhsMimeType.Equals(rhsMimeType, StringComparison.OrdinalIgnoreCase);

            // If we have a generic MIME type, check for matching file extensions.

            return Extensions.Any(ext => other.Extensions.Any(otherExt => otherExt.Equals(ext, StringComparison.OrdinalIgnoreCase)));

        }

        public override int GetHashCode() {

            string fileTypeStr = GetMimeTypeString(this)?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(fileTypeStr))
                fileTypeStr = Extensions?.FirstOrDefault()?.ToLowerInvariant();

            return (fileTypeStr ?? string.Empty).GetHashCode();

        }
        public override string ToString() {

            return MimeTypes?.FirstOrDefault()?.ToString();

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

        // Private members

        private static string GetMimeTypeString(IFileFormat fileFormat) {

            return (fileFormat?.MimeTypes?.FirstOrDefault()?.ToString() ?? string.Empty).ToLowerInvariant();

        }

    }

}
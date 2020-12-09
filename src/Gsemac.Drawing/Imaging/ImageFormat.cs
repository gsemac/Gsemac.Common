using System;

namespace Gsemac.Drawing.Imaging {

    public class ImageFormat :
        IImageFormat,
        IComparable,
        IComparable<IImageFormat> {

        // Public members

        public string FileExtension => fileExtension;

        public static IImageFormat FromFileExtension(string fileExtension) {

            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentNullException(nameof(fileExtension));

            if (!fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;

            fileExtension = fileExtension.Trim().ToLowerInvariant();

            return new ImageFormat(fileExtension);

        }

        public override bool Equals(object obj) {

            if (obj is IImageFormat imageFormat)
                return new ImageFormatComparer().Equals(this, imageFormat);
            else
                return false;

        }
        public override int GetHashCode() {

            return new ImageFormatComparer().GetHashCode(this);

        }
        public override string ToString() {

            return FileExtension;

        }

        public int CompareTo(IImageFormat other) {

            if (other is null)
                return 1;

            return FileExtension.CompareTo(other.FileExtension);

        }
        public int CompareTo(object obj) {

            if (obj is IImageFormat other)
                return CompareTo(other);

            throw new ArgumentException(nameof(obj));

        }

        // Private members

        private readonly string fileExtension;

        private ImageFormat(string fileExtension) {

            this.fileExtension = fileExtension;

        }

    }

}
using Gsemac.IO;
using System;

namespace Gsemac.Drawing.Imaging {

    public class ImageFormat :
        IImageFormat,
        IComparable,
        IComparable<IImageFormat> {

        // Public members

        public string FileExtension => fileExtension;

        public static IImageFormat Jpeg => FromFileExtension(".jpeg");
        public static IImageFormat Png => FromFileExtension(".png");

        public static IImageFormat FromFileExtension(string filePath) {

            // Accepts full file paths, or plain image extensions (with or without leading period, e.g. ".jpeg" and "jpeg").

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(filePath);

            string ext = PathUtilities.GetFileExtension(filePath);

            if (string.IsNullOrWhiteSpace(ext))
                ext = filePath;

            if (!ext.StartsWith("."))
                ext = "." + ext;

            ext = ext.Trim().ToLowerInvariant();

            return new ImageFormat(ext);

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
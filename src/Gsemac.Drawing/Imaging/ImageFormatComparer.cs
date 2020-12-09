using System;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public class ImageFormatComparer :
        EqualityComparer<IImageFormat> {

        // Public members

        public override bool Equals(IImageFormat x, IImageFormat y) {

            return x.GetHashCode() == y.GetHashCode();

        }
        public override int GetHashCode(IImageFormat obj) {

            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            string fileExtension = obj.FileExtension;

            if (string.IsNullOrWhiteSpace(fileExtension))
                return "".GetHashCode();

            fileExtension = NormalizeFileExtension(fileExtension);

            return fileExtension.GetHashCode();

        }

        // Private members

        private string NormalizeFileExtension(string fileExtension) {

            fileExtension = fileExtension.Trim().ToLowerInvariant();

            if (!fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;

            switch (fileExtension) {

                case ".jpg":
                    return ".jpeg";

                case ".tif":
                    return ".tiff";

                default:
                    return fileExtension;

            }

        }

    }

}
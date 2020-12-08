using System;

namespace Gsemac.Drawing.Imaging {

    public class ImageFormat :
        IImageFormat {

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

        // Private members

        private readonly string fileExtension;

        private ImageFormat(string fileExtension) {

            this.fileExtension = fileExtension;

        }

    }

}
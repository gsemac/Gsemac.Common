#if NETFRAMEWORK

using Gsemac.IO;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageConverter :
            IImageConverter {

        // Public members

        public void ConvertImage(string sourceFilePath, string destinationFilePath, IImageConversionOptions options) {

            if (string.IsNullOrEmpty(sourceFilePath))
                throw new ArgumentNullException(nameof(sourceFilePath));

            if (string.IsNullOrEmpty(destinationFilePath))
                throw new ArgumentNullException(nameof(destinationFilePath));

            sourceFilePath = Path.GetFullPath(sourceFilePath);
            destinationFilePath = Path.GetFullPath(destinationFilePath);

            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("The source file was not found.", sourceFilePath);

            string sourceExt = Path.GetExtension(sourceFilePath);
            string destinationExt = Path.GetExtension(destinationFilePath);
            bool overwriteSourceFile = sourceFilePath.Equals(destinationFilePath, StringComparison.OrdinalIgnoreCase);

            if (!ImageReader.IsSupportedFileType(sourceExt) || !ImageReader.IsSupportedFileType(destinationExt))
                throw new FileFormatException("The image format is not supported.");

            if (sourceExt.Equals(destinationExt, StringComparison.OrdinalIgnoreCase) &&
                options.EncoderOptions.Quality == ImageEncoderOptions.BestQuality &&
                !options.Filters.Any()) {

                // The image is being converted to the same format without any changes.
                // Instead of re-encoding, just copy the image to its new location.

                if (!overwriteSourceFile)
                    File.Copy(sourceFilePath, destinationFilePath, overwrite: true);

            }
            else {

                // Re-encode the image using the specified settings.

                Image image;

                using (image = ImageUtilities.OpenImage(sourceFilePath, openWithoutLocking: overwriteSourceFile)) {

                    image = ImageFilter.ApplyAll(image, options.Filters);

                    ImageUtilities.SaveImage(image, destinationFilePath, options.EncoderOptions);

                }

            }

        }

    }

}

#endif
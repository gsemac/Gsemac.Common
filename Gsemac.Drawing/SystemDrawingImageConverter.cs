#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing {

    public class SystemDrawingImageConverter :
            IImageConverter {

        // Public members

        public IEnumerable<string> SupportedImageFormats => new[] {
            ".bmp",
            ".gif",
            ".exif",
            ".jpg",
            ".jpeg",
            ".png",
            ".tif",
            ".tiff"
        };

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

            if (!SupportedImageFormats.Any(ext => ext.Equals(sourceExt, StringComparison.OrdinalIgnoreCase)) ||
                !SupportedImageFormats.Any(ext => ext.Equals(destinationExt, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("The image format is not supported.");

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, (int)(100.0f * options.Quality))) {

                encoderParameters.Param[0] = qualityParameter;

                ImageCodecInfo encoder = GetEncoderForFileExtension(destinationExt);

                Image image;

                using (image = ImageUtilities.OpenImage(sourceFilePath, openWithoutLocking: overwriteSourceFile)) {

                    foreach (IImageFilter filter in options.Filters)
                        image = filter.Apply(image);

                    image.Save(destinationFilePath, encoder, encoderParameters);

                }

            }

        }

        // Private members

        private ImageFormat GetImageFormatForFileExtension(string fileExtension) {

            switch (fileExtension.ToLowerInvariant()) {

                case ".bmp":
                    return ImageFormat.Bmp;

                case ".gif":
                    return ImageFormat.Gif;

                case ".exif":
                    return ImageFormat.Exif;

                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;

                default:
                    throw new ArgumentException("Unrecognized file extension.");

            }

        }
        private ImageCodecInfo GetEncoderForFileExtension(string fileExtension) {

            ImageFormat format = GetImageFormatForFileExtension(fileExtension);

            ImageCodecInfo decoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == format.Guid)
                .FirstOrDefault();

            return decoder;

        }

    }

}

#endif
#if NETFRAMEWORK

using Gsemac.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageReaderExtensions {

        public static bool IsSupportedFileType(this IImageReader imageReader, string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return imageReader.SupportedFileTypes.Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }
        public static Image ReadImage(this IImageReader imageReader, string filePath) {

            if (!imageReader.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenRead(filePath))
                return imageReader.ReadImage(fs);

        }
        public static void SaveImage(this IImageReader imageReader, Image image, string filePath, IImageEncoderOptions options) {

            if (!imageReader.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenWrite(filePath))
                imageReader.SaveImage(image, fs, options);

        }
        public static void SaveImage(this NativeImageReader imageReader, Image image, string filePath, ImageFormat imageFormat, IImageEncoderOptions options) {

            if (!imageReader.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenWrite(filePath))
                imageReader.SaveImage(image, fs, imageFormat, options);

        }

    }

}

#endif
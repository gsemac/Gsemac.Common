#if NETFRAMEWORK

using Gsemac.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageReaderExtensions {

        public static bool IsSupportedFileType(this IImageCodec imageCodec, string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return imageCodec.SupportedFileTypes.Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }

        public static void Encode(this IImageCodec imageCodec, Image image, string filePath, IImageEncoderOptions options) {

            if (!imageCodec.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenWrite(filePath))
                imageCodec.Encode(image, fs, options);

        }
        public static void Encode(this NativeImageCodec imageCodec, Image image, string filePath, ImageFormat imageFormat, IImageEncoderOptions options) {

            if (!imageCodec.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenWrite(filePath))
                imageCodec.Encode(image, fs, imageFormat, options);

        }
        public static Image Decode(this IImageCodec imageCodec, string filePath) {

            if (!imageCodec.IsSupportedFileType(filePath))
                throw new FileFormatException("The image format is not supported.");

            using (FileStream fs = File.OpenRead(filePath))
                return imageCodec.Decode(fs);

        }

    }

}

#endif
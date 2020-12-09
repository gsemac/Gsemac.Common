using Gsemac.IO;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecExtensions {

        public static bool IsSupportedImageFormat(this IImageCodec imageCodec, string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return imageCodec.IsSupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(this IImageCodec imageCodec, IImageFormat imageFormat) {

            return imageCodec.SupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

        public static void Encode(this IImageCodec imageCodec, IImage image, string filePath, IImageEncoderOptions options) {

            if (!imageCodec.IsSupportedImageFormat(filePath))
                throw ImageExceptions.UnsupportedImageFormat;

            using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                imageCodec.Encode(image, fs, options);

        }
        public static IImage Decode(this IImageCodec imageCodec, string filePath) {

            if (!imageCodec.IsSupportedImageFormat(filePath))
                throw ImageExceptions.UnsupportedImageFormat;

            using (FileStream fs = File.OpenRead(filePath))
                return imageCodec.Decode(fs);

        }

    }

}
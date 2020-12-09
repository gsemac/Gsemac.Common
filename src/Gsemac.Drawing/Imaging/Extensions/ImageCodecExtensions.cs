using Gsemac.Drawing.Imaging.Internal;
using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecExtensions {

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
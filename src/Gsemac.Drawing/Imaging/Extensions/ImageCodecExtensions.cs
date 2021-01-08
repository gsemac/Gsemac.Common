using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecExtensions {

        public static void Encode(this IImageCodec imageCodec, IImage image, Stream stream) {

            imageCodec.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageCodec imageCodec, IImage image, string filePath) {

            imageCodec.Encode(image, filePath, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageCodec imageCodec, IImage image, string filePath, IImageEncoderOptions options) {

            if (!imageCodec.IsSupportedFileFormat(filePath))
                throw new UnsupportedFileFormatException();

            using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                imageCodec.Encode(image, fs, options);

        }
        public static IImage Decode(this IImageCodec imageCodec, string filePath) {

            if (!imageCodec.IsSupportedFileFormat(filePath))
                throw new UnsupportedFileFormatException();

            using (FileStream fs = File.OpenRead(filePath))
                return imageCodec.Decode(fs);

        }

    }

}
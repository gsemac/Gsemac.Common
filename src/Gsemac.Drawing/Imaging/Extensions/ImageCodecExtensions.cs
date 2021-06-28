using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecExtensions {

        public static void Encode(this IImageEncoder encoder, IImage image, Stream stream) {

            encoder.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageEncoder encoder, IImage image, string filePath) {

            encoder.Encode(image, filePath, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageEncoder encoder, IImage image, string filePath, IImageEncoderOptions options) {

            if (!encoder.IsSupportedFileFormat(filePath))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                encoder.Encode(image, fs, options);

        }
        public static IImage Decode(this IImageDecoder decoder, string filePath) {

            if (!decoder.IsSupportedFileFormat(filePath))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            using (FileStream fs = File.OpenRead(filePath))
                return decoder.Decode(fs);

        }

    }

}
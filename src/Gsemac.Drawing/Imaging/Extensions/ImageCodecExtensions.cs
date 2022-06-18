using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecExtensions {

        // Public members

        public static void Encode(this IImageEncoder encoder, IImage image, Stream stream) {

            encoder.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageEncoder encoder, IImage image, string filePath) {

            encoder.Encode(image, filePath, ImageEncoderOptions.Default);

        }
        public static void Encode(this IImageEncoder encoder, IImage image, string filePath, IImageEncoderOptions options) {

            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!encoder.IsSupportedFileFormat(filePath))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate))
                encoder.Encode(image, fs, options);

        }

        public static IImage Decode(this IImageDecoder decoder, Stream stream) {

            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return decoder.Decode(stream, ImageDecoderOptions.Default);

        }
        public static IImage Decode(this IImageDecoder decoder, string filePath) {

            return Decode(decoder, filePath, ImageDecoderOptions.Default);

        }
        public static IImage Decode(this IImageDecoder decoder, string filePath, IImageDecoderOptions options) {

            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            if (!decoder.IsSupportedFileFormat(filePath))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            using (FileStream fs = File.OpenRead(filePath))
                return decoder.Decode(fs, options);

        }

    }

}
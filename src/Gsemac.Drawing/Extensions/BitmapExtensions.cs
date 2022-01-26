#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class BitmapExtensions {

        // Public members

        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat, IImageEncoder encoder, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            using (FileStream fs = File.OpenWrite(filePath))
                Save(bitmap, fs, imageFormat, encoder, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            IImageCodec imageCodec = ImageCodecFactory.Default.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, filePath, imageFormat, imageCodec, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat) {

            Save(bitmap, filePath, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this Image bitmap, string filePath, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            IFileFormat imageFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            if (imageFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, filePath, imageFormat, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath) {

            Save(bitmap, filePath, ImageEncoderOptions.Default);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat, IImageEncoder encoder, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            encoder.Encode(ImageFactory.FromBitmap(bitmap), stream, encoderOptions);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            IImageCodec imageCodec = ImageCodecFactory.Default.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, stream, imageFormat, imageCodec, encoderOptions);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat) {

            Save(bitmap, stream, imageFormat, ImageEncoderOptions.Default);

        }

    }

}

#endif
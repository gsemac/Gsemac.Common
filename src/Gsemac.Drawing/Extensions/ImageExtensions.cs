using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        // Public members

        public static bool IsAnimated(this IImage image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return image.Animation?.FrameCount > 1;

        }

        public static void Save(this IImage image, Stream stream) {

            image.Codec.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat imageFormat) {

            image.Save(stream, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            // Attempt to use the same codec the image was created with if it supports the given image format.

            IImageEncoder encoder = (image.Codec is object && image.Codec.IsSupportedFileFormat(imageFormat)) ?
                image.Codec :
                ImageCodecFactory.Default.FromFileFormat(imageFormat);

            if (encoder is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            encoder.Encode(image, stream, encoderOptions);

            if (stream.CanSeek && encoderOptions.OptimizationMode != ImageOptimizationMode.None) {

                IImageOptimizer imageOptimizer = ImageOptimizerFactory.Default.FromFileFormat(imageFormat);

                if (!(imageOptimizer is null)) {

                    stream.Seek(0, SeekOrigin.Begin);

                    imageOptimizer.Optimize(stream, encoderOptions.OptimizationMode);

                }

            }

        }

        public static void Save(this IImage image, string filePath) {

            image.Save(filePath, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, string filePath, IFileFormat imageFormat) {

            image.Save(filePath, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, string filePath, IImageEncoderOptions encoderOptions) {

            IFileFormat imageFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            if (imageFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            image.Save(filePath, imageFormat, encoderOptions);

        }
        public static void Save(this IImage image, string filePath, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            using (FileStream stream = File.Open(filePath, FileMode.OpenOrCreate))
                image.Save(stream, imageFormat, encoderOptions);

        }

#if NETFRAMEWORK
        public static Bitmap ToBitmap(this IImage image, bool disposeSourceImage) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            Bitmap bitmap = image.ToBitmap();

            if (disposeSourceImage)
                image.Dispose();

            return bitmap;

        }
#endif

    }

}
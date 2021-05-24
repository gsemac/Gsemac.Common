using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        // Public members

        public static void Save(this IImage image, Stream stream) {

            image.Codec.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat imageFormat) {

            image.Save(stream, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            IImageEncoder encoder = ImageCodecFactory.Default.FromFileFormat(imageFormat);

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
        public static void Save(this IImage image, string filePath, IImageEncoderOptions encoderOptions) {

            IFileFormat imageFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            if (imageFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            image.Save(filePath, imageFormat, encoderOptions);

        }
        public static void Save(this IImage image, string filePath, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            using (FileStream stream = File.Open(filePath, FileMode.OpenOrCreate))
                image.Save(stream, imageFormat, encoderOptions);

        }

    }

}
using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        // Public members

        public static bool IsAnimated(this IImage image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return image.FrameCount > 1;

        }

        public static void Save(this IImage image, Stream stream) {

            image.Codec.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat format) {

            image.Save(stream, format, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IFileFormat format, IImageEncoderOptions options) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            IImageEncoder encoder = ImageCodecFactory.Default.FromFileFormat(format);

            if (encoder is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            encoder.Encode(image, stream, options);

            if (stream.CanSeek && options.OptimizationMode != OptimizationMode.None) {

                IImageOptimizer imageOptimizer = ImageOptimizerFactory.Default.FromFileFormat(format);

                if (imageOptimizer is object) {

                    stream.Seek(0, SeekOrigin.Begin);

                    imageOptimizer.Optimize(stream, options.OptimizationMode);

                }

            }

        }

        public static void Save(this IImage image, string filePath) {

            image.Save(filePath, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, string filePath, IFileFormat format) {

            image.Save(filePath, format, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, string filePath, IImageEncoderOptions options) {

            IFileFormat imageFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            if (imageFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            image.Save(filePath, imageFormat, options);

        }
        public static void Save(this IImage image, string filePath, IFileFormat format, IImageEncoderOptions options) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.AddFileExtension && format.Extensions.Any() && !format.Extensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                filePath += format.Extensions.First();

            using (FileStream stream = File.Open(filePath, FileMode.OpenOrCreate))
                image.Save(stream, format, options);

        }

        public static Image ToBitmap(this IImage image, IBitmapOptions options) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Bitmap bitmap = image.ToBitmap();

            if (options.DisposeSourceImage)
                image.Dispose();

            return bitmap;

        }

    }

}
using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Internal;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        // Public members

        public static void Save(this IImage image, Stream stream) {

            image.Codec.Encode(image, stream, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IImageFormat imageFormat) {

            image.Save(stream, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this IImage image, Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            IImageEncoder encoder = ImageCodec.FromImageFormat(imageFormat);

            if (encoder is null)
                throw ImageExceptions.UnsupportedImageFormat;

            image.Codec.Encode(image, stream, encoderOptions);

            if (stream.CanSeek && encoderOptions.OptimizationMode != ImageOptimizationMode.None) {

                IImageOptimizer imageOptimizer = ImageOptimizer.FromImageFormat(imageFormat);

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

            IImageFormat imageFormat = ImageFormat.FromFileExtension(filePath);

            if (imageFormat is null)
                throw ImageExceptions.UnsupportedImageFormat;

            using (FileStream stream = File.Open(filePath, FileMode.OpenOrCreate))
                image.Save(stream, imageFormat, encoderOptions);

        }

#if NETFRAMEWORK

        public static bool HasIndexedPixelFormat(this System.Drawing.Image image) {

            return image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Indexed;

        }

#endif

    }

}
using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;
using Gsemac.IO;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public static class ImageUtilities {

        // Public members

        public static IImage OpenImage(string filePath) {

            return OpenImageInternal(filePath);

        }
        public static void SaveImage(IImage image, string filePath, IImageEncoderOptions options = null) {

            SaveImageInternal(image, filePath, options ?? new ImageEncoderOptions());

        }

#if NETFRAMEWORK

        public static void SaveImage(Image image, string filePath, IImageEncoderOptions options = null) {

            SaveImageInternal(new GdiImage(image), filePath, options ?? new ImageEncoderOptions());

        }
        public static Image ResizeImage(Image image, int? width = null, int? height = null, bool disposeOriginal = false) {

            int newWidth = image.Width;
            int newHeight = image.Height;

            if (width.HasValue && height.HasValue) {

                newWidth = width.Value;
                newHeight = height.Value;

            }
            else if (width.HasValue) {

                float scaleFactor = (float)width.Value / image.Width;

                newWidth = width.Value;
                newHeight = (int)(image.Height * scaleFactor);

            }
            else if (height.HasValue) {

                float scaleFactor = (float)height.Value / image.Height;

                newWidth = (int)(image.Width * scaleFactor);
                newHeight = height.Value;

            }

            Bitmap resultImage = new Bitmap(image, new Size(newWidth, newHeight));

            if (disposeOriginal)
                image.Dispose();

            return resultImage;

        }
        public static Image ConvertImageToNonIndexedPixelFormat(Image image, bool disposeOriginal = false) {

            // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

            if (!image.HasIndexedPixelFormat())
                return image;

            Bitmap resultImage = new Bitmap(image);

            if (disposeOriginal)
                image.Dispose();

            return resultImage;

        }
        public static Image ConvertImageToNonIndexedPixelFormat(IImage image, bool disposeOriginal = false) {

            Bitmap resultImage = image.ToBitmap();

            if (disposeOriginal)
                image.Dispose();

            return ConvertImageToNonIndexedPixelFormat(resultImage, disposeOriginal: true);

        }

#endif

        // Private members

        private static IImage OpenImageInternal(string filePath) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw ImageExceptions.UnsupportedImageFormat;

            return imageCodec.Decode(filePath);

        }
        private static void SaveImageInternal(IImage image, string filePath, IImageEncoderOptions options) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw ImageExceptions.UnsupportedImageFormat;

            imageCodec.Encode(image, filePath, options);

        }

    }

}
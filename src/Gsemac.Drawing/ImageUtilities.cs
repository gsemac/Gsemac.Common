#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System.Drawing;

namespace Gsemac.Drawing {

    public static class ImageUtilities {

        // Public members
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

        public static bool HasIndexedPixelFormat(Image image) {

            return image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Indexed;

        }
        public static Image ConvertImageToNonIndexedPixelFormat(Image image, bool disposeOriginal = false) {

            // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

            if (!HasIndexedPixelFormat(image))
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

        public static IImage CreateImageFromBitmap(Bitmap bitmap) {

            return CreateImageFromBitmap((Image)bitmap);

        }
        public static IImage CreateImageFromBitmap(Image bitmap) {

            return CreateImageFromBitmap(bitmap, null, null);

        }
        public static IImage CreateImageFromBitmap(Image bitmap, IFileFormat imageFormat, IImageCodec imageCodec) {

            return new GdiImage(bitmap, imageFormat, imageCodec);

        }

    }

}

#endif
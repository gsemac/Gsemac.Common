#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System.Drawing;

namespace Gsemac.Drawing {

    public static class ImageUtilities {

        // Public members
        public static Image ResizeImage(Image image, int? width = null, int? height = null, bool disposeSourceImage = false) {

            if ((width ?? 0) <= 0)
                width = null;

            if ((height ?? 0) <= 0)
                height = null;

            int newWidth;
            int newHeight;

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
            else {

                // If no dimensions have been specified, simply return the original image.

                return image;

            }

            Bitmap resultImage = new Bitmap(image, new Size(newWidth, newHeight));

            if (disposeSourceImage)
                image.Dispose();

            return resultImage;

        }

        public static bool HasIndexedPixelFormat(Image image) {

            switch (image.PixelFormat) {

                case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
                case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                case System.Drawing.Imaging.PixelFormat.Indexed:
                    return true;

                default:
                    return false;

            }

        }
        public static Image ConvertImageToNonIndexedPixelFormat(Image image, bool disposeSourceImage = false) {

            // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

            if (!HasIndexedPixelFormat(image))
                return image;

            Bitmap resultImage = new Bitmap(image);

            if (disposeSourceImage)
                image.Dispose();

            return resultImage;

        }
        public static Image ConvertImageToNonIndexedPixelFormat(IImage image, bool disposeSourceImage = false) {

            Bitmap resultImage = image.ToBitmap();

            if (disposeSourceImage)
                image.Dispose();

            return ConvertImageToNonIndexedPixelFormat(resultImage, disposeSourceImage: true);

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
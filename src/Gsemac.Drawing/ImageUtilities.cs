#if NETFRAMEWORK

using System.Drawing;

namespace Gsemac.Drawing {

    public static class ImageUtilities {

        // Public members

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
        public static Image ConvertToNonIndexedPixelFormat(Image image, bool disposeSourceImage = false) {

            // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

            if (!HasIndexedPixelFormat(image))
                return image;

            Bitmap resultImage = new Bitmap(image);

            if (disposeSourceImage)
                image.Dispose();

            return resultImage;

        }
        public static Image ConvertToNonIndexedPixelFormat(IImage image, bool disposeSourceImage = false) {

            Bitmap resultImage = image.ToBitmap();

            if (disposeSourceImage)
                image.Dispose();

            return ConvertToNonIndexedPixelFormat(resultImage, disposeSourceImage: true);

        }

    }

}

#endif
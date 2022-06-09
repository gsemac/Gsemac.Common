using Gsemac.Drawing.Imaging;
using System;
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

        public static Image Resize(Image image, IImageResizingOptions options) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!options.Width.HasValue && !options.Height.HasValue && !options.HorizontalScale.HasValue && !options.VerticalScale.HasValue)
                return (Image)image.Clone();

            if (options.SizingMode == ImageSizingMode.ResizeIfLarger) {

                if ((!options.Width.HasValue || image.Width <= options.Width.Value) && (!options.Height.HasValue || image.Height <= options.Height.Value))
                    return (Image)image.Clone();

            }

            if (options.SizingMode == ImageSizingMode.ResizeIfSmaller) {

                if ((!options.Width.HasValue || image.Width >= options.Width.Value) && (!options.Height.HasValue || image.Height >= options.Height.Value))
                    return (Image)image.Clone();

            }

            int? newWidth = options.Width;
            int? newHeight = options.Height;

            if (!newWidth.HasValue && options.HorizontalScale.HasValue)
                newWidth = (int)(image.Width * options.HorizontalScale.Value);

            if (!newHeight.HasValue && options.VerticalScale.HasValue)
                newHeight = (int)(image.Height * options.VerticalScale.Value);

            // If the image hasn't been resized at all, just return the source image.

            if (!newWidth.HasValue && !newHeight.HasValue)
                return (Image)image.Clone();

            return ResizeInternal(image, width: newWidth, height: newHeight);

        }
        public static Image Resize(Image image, int? width = null, int? height = null) {

            return Resize(image, new ImageResizingOptions() {
                Width = width,
                Height = height,
            });

        }
        public static Image Resize(Image image, float? horizontalScale = null, float? verticalScale = null) {

            return Resize(image, new ImageResizingOptions() {
                HorizontalScale = horizontalScale,
                VerticalScale = verticalScale,
            });

        }

        // Private members

        private static Image ResizeInternal(Image image, int? width = null, int? height = null) {

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

            return resultImage;

        }

    }

}
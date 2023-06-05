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

            // If new dimensions haven't been provided, then scaling takes secondary priority.

            if (!newWidth.HasValue && options.HorizontalScale.HasValue)
                newWidth = (int)(image.Width * options.HorizontalScale.Value);

            if (!newHeight.HasValue && options.VerticalScale.HasValue)
                newHeight = (int)(image.Height * options.VerticalScale.Value);

            // Adjust one of the dimensions in order to maintain the original aspect ratio if necessary.

            if (newWidth.HasValue && newHeight.HasValue && options.MaintainAspectRatio) {

                double newHorizontalScale = (double)newWidth.Value / image.Width;
                double newVerticalScale = (double)newHeight.Value / image.Height;
                double minimumScale = Math.Min(newHorizontalScale, newVerticalScale);

                newWidth = (int)(image.Width * minimumScale);
                newHeight = (int)(image.Height * minimumScale);

            }

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
        public static Image Resize(Image image, double? horizontalScale = null, double? verticalScale = null) {

            return Resize(image, new ImageResizingOptions() {
                HorizontalScale = horizontalScale,
                VerticalScale = verticalScale,
            });

        }

        public static Bitmap Trim(Bitmap image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return Trim(image, 0.0);

        }
        public static Bitmap Trim(Bitmap image, Color trimColor) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return Trim(image, trimColor, 0.0);

        }
        public static Bitmap Trim(Bitmap image, double tolerance) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            Color trimColor = image.GetPixel(0, 0);

            return Trim(image, trimColor, tolerance);

        }
        public static Bitmap Trim(Bitmap image, Color trimColor, double tolerance) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (image.Width > 0 && image.Height > 0) {

                // Calculate the area to trim.

                int left, right, top, bottom;

                for (left = 0; left < image.Width; ++left)
                    if (!ColumnIsColor(image, left, trimColor, tolerance))
                        break;

                for (right = image.Width - 1; right >= 0; --right)
                    if (!ColumnIsColor(image, right, trimColor, tolerance))
                        break;

                for (top = 0; top < image.Height; ++top)
                    if (!RowIsColor(image, top, trimColor, tolerance))
                        break;

                for (bottom = image.Height - 1; bottom >= 0; --bottom)
                    if (!RowIsColor(image, bottom, trimColor, tolerance))
                        break;

                // Crop the image.

                int x = left;
                int y = top;
                int width = right + 1 - x;
                int height = bottom + 1 - y;

                // If the cropped area is impossible (this can occur if the tolerance is too high or the image is entirely cropped), use the original dimensions.

                if (width <= 0) {

                    x = 0;
                    width = image.Width;

                }

                if (height <= 0) {

                    y = 0;
                    height = image.Height;

                }

                Bitmap newImage = new Bitmap(width, height);

                try {

                    using (Graphics graphics = Graphics.FromImage(newImage)) {

                        graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);

                        return newImage;

                    }

                }
                catch (Exception) {

                    newImage.Dispose();

                    throw;

                }

            }
            else {

                return image;

            }

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

            // Don't allow any dimension to be reduced to 0, because we'll get an exception.
            // TODO: Add a unit test for this.

            if (newWidth == 0)
                newWidth = 1;

            if (newHeight == 0)
                newHeight = 1;

            Bitmap resultImage = new Bitmap(image, new Size(newWidth, newHeight));

            return resultImage;

        }

        private static bool ColorIsMatch(Color first, Color second, double tolerance) {

            double requiredSimilarity = 1.0 - tolerance;

            return ColorUtilities.ComputeSimilarity(first, second, ColorDistanceStrategy.DeltaE) >= requiredSimilarity;

        }
        private static bool RowIsColor(Bitmap bitmap, int row, Color trimColor, double tolerance) {

            for (int x = 0; x < bitmap.Width; ++x)
                if (!ColorIsMatch(bitmap.GetPixel(x, row), trimColor, tolerance))
                    return false;

            return true;

        }
        private static bool ColumnIsColor(Bitmap bitmap, int column, Color trimColor, double tolerance) {

            for (int y = 0; y < bitmap.Height; ++y)
                if (!ColorIsMatch(bitmap.GetPixel(column, y), trimColor, tolerance))
                    return false;

            return true;

        }

    }

}
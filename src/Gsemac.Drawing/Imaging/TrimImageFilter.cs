#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing.Imaging {

    public class TrimImageFilter :
        IImageFilter {

        // Public members

        public TrimImageFilter() {
        }
        public TrimImageFilter(double tolerance) {

            this.tolerance = tolerance;

        }
        public TrimImageFilter(double tolerance, IColorDistanceAlgorithm distanceAlgorithm) {

            this.tolerance = tolerance;
            this.distanceAlgorithm = distanceAlgorithm;

        }
        public TrimImageFilter(Color trimColor, double tolerance) {

            this.trimColor = trimColor;
            this.tolerance = tolerance;

        }
        public TrimImageFilter(Color trimColor, double tolerance, IColorDistanceAlgorithm distanceAlgorithm) {

            this.trimColor = trimColor;
            this.distanceAlgorithm = distanceAlgorithm;
            this.tolerance = tolerance;

        }
        public TrimImageFilter(Color trimColor, IColorDistanceAlgorithm distanceAlgorithm) {

            this.trimColor = trimColor;
            this.distanceAlgorithm = distanceAlgorithm;

        }

        public Image Apply(Image sourceImage) {

            if (sourceImage is Bitmap sourceBitmap && sourceBitmap.Width > 0 && sourceBitmap.Height > 0) {

                // Get the trim color if the user didn't specify one.

                if (!trimColor.HasValue)
                    trimColor = sourceBitmap.GetPixel(0, 0);

                // Calculate the area to trim.

                int left, right, top, bottom;

                for (left = 0; left < sourceBitmap.Width; ++left)
                    if (!ColumnIsColor(sourceBitmap, left, trimColor.Value))
                        break;

                for (right = sourceBitmap.Width - 1; right >= 0; --right)
                    if (!ColumnIsColor(sourceBitmap, right, trimColor.Value))
                        break;

                for (top = 0; top < sourceBitmap.Height; ++top)
                    if (!RowIsColor(sourceBitmap, top, trimColor.Value))
                        break;

                for (bottom = sourceBitmap.Height - 1; bottom >= 0; --bottom)
                    if (!RowIsColor(sourceBitmap, bottom, trimColor.Value))
                        break;

                // Crop the image.

                int x = left;
                int y = top;
                int width = right + 1 - x;
                int height = bottom + 1 - y;

                if (sourceImage.HasIndexedPixelFormat()) {

                    // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

                    using (sourceImage)
                        sourceBitmap = new Bitmap(sourceImage);

                }

                Bitmap result = new Bitmap(width, height);

                using (sourceBitmap)
                using (Graphics graphics = Graphics.FromImage(result)) {

                    try {

                        graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);

                    }
                    catch (Exception ex) {

                        result.Dispose();

                        throw ex;

                    }

                    return result;

                }

            }
            else
                return sourceImage;

        }

        // Private members

        private const double defaultTolerance = 0.1;

        private Color? trimColor;
        private readonly IColorDistanceAlgorithm distanceAlgorithm = new DeltaEColorDistanceAlgorithm();
        private readonly double tolerance = defaultTolerance;

        private bool CompareColors(Color first, Color second) {

            return ColorUtilities.ColorDistance(first, second, distanceAlgorithm) <= tolerance;

        }
        private bool RowIsColor(Bitmap bitmap, int row, Color trimColor) {

            for (int x = 0; x < bitmap.Width; ++x)
                if (!CompareColors(bitmap.GetPixel(x, row), trimColor))
                    return false;

            return true;

        }
        private bool ColumnIsColor(Bitmap bitmap, int column, Color trimColor) {

            for (int y = 0; y < bitmap.Height; ++y)
                if (!CompareColors(bitmap.GetPixel(column, y), trimColor))
                    return false;

            return true;

        }

    }

}

#endif
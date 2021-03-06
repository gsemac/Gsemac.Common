﻿#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public class TrimImageFilter :
        IImageFilter {

        // Public members

        public TrimImageFilter() {
        }
        public TrimImageFilter(int tolerance) {

            this.tolerance = tolerance;

        }
        public TrimImageFilter(int tolerance, IColorDistanceStrategy distanceAlgorithm) {

            this.tolerance = tolerance;
            this.distanceAlgorithm = distanceAlgorithm;

        }
        public TrimImageFilter(Color trimColor, int tolerance) {

            this.trimColor = trimColor;
            this.tolerance = tolerance;

        }
        public TrimImageFilter(Color trimColor, int tolerance, IColorDistanceStrategy distanceAlgorithm) {

            this.trimColor = trimColor;
            this.distanceAlgorithm = distanceAlgorithm;
            this.tolerance = tolerance;

        }
        public TrimImageFilter(Color trimColor, IColorDistanceStrategy distanceAlgorithm) {

            this.trimColor = trimColor;
            this.distanceAlgorithm = distanceAlgorithm;

        }

        public IImage Apply(IImage image) {

            Image newImage = null;

            try {

                using (Bitmap sourceBitmap = (Bitmap)ImageUtilities.ConvertImageToNonIndexedPixelFormat(image)) {

                    if (sourceBitmap.Width > 0 && sourceBitmap.Height > 0) {

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

                        // If the cropped area is impossible (this can occur if the tolerance is too high or the image is entirely cropped), use the original dimensions.

                        if (width <= 0) {

                            x = 0;
                            width = image.Width;

                        }

                        if (height <= 0) {

                            y = 0;
                            height = image.Height;

                        }

                        newImage = new Bitmap(width, height);

                        using (Graphics graphics = Graphics.FromImage(newImage)) {

                            graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);

                            return ImageUtilities.CreateImageFromBitmap(newImage);

                        }

                    }
                    else
                        return image;

                }

            }
            catch (Exception) {

                if (newImage is object)
                    newImage.Dispose();

                throw;

            }

        }

        // Private members

        private const int defaultTolerance = 10;

        private Color? trimColor;
        private readonly IColorDistanceStrategy distanceAlgorithm = new DeltaEColorDistanceStrategy();
        private readonly int tolerance = defaultTolerance;

        private bool CompareColors(Color first, Color second) {

            return ColorUtilities.ComputeDistance(first, second, distanceAlgorithm) <= tolerance / 100.0;

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
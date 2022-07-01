using Gsemac.Core.Extensions;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public static class ColorUtilities {

        // Public members

        public static double ComputeDistance(Color first, Color second) {

            return ComputeDistance(first, second, ColorDistanceStrategy.DeltaE);

        }
        public static double ComputeDistance(Color first, Color second, IColorDistanceStrategy strategy) {

            if (strategy is null)
                throw new ArgumentNullException(nameof(strategy));

            return strategy.ComputeDistance(first, second);

        }

        public static double ComputeSimilarity(Color first, Color second) {

            return ComputeSimilarity(first, second, ColorDistanceStrategy.DeltaE);

        }
        public static double ComputeSimilarity(Color first, Color second, IColorDistanceStrategy strategy) {

            if (strategy is null)
                throw new ArgumentNullException(nameof(strategy));

            return strategy.ComputeSimilarity(first, second);

        }

        /// <summary>
        /// Returns a luminance value between 0.0 and 1.0 for the given SRGB color.
        /// </summary>
        /// <param name="color">The SRGB color to compute the luminance value for.</param>
        /// <returns>A luminance value between 0.0 and 1.0.</returns>
        public static double ComputeRelativeLuminance(Color color) {

            // https://stackoverflow.com/a/56678483/5383169 (Myndex)

            // Map all RGB components to the range [0.0, 1.0].

            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            // Convert the gamma-encoded RGB to a linear value.

            r = SrgbToLinear(r);
            g = SrgbToLinear(g);
            b = SrgbToLinear(b);

            // Apply the standard coeffients to calculate luminance.

            double y = 0.2126 * r + 0.7152 * g + 0.0722 * b;

            return y;

        }
        /// <summary>
        /// Returns a lightness value (L*) between 0.0 (black) and 1.0 (white) for the given SRGB color.
        /// </summary>
        /// <param name="color">The SRGB color to compute the lightness value for.</param>
        /// <returns>A lightness value between 0.0 and 1.0.</returns>
        public static double ComputePerceivedLightness(Color color) {

            // https://stackoverflow.com/a/56678483/5383169 (Myndex)

            double luminance = ComputeRelativeLuminance(color);
            double perceivedLightness = LuminanceToLStar(luminance);

            return perceivedLightness / 100.0;

        }

        public static Color ToGreyscale(Color color) {

            int greyColor = (int)((0.11 * color.R) + (0.59 * color.G) + (0.30 * color.B));

            return Color.FromArgb(greyColor, greyColor, greyColor);

        }

        // Private members

        private static double SrgbToLinear(double srgbChannel) {

            if (srgbChannel <= 0.04045)
                return srgbChannel / 12.92;

            return Math.Pow((srgbChannel + 0.055) / 1.055, 2.4);

        }
        private static double LuminanceToLStar(double luminance) {

            // Send this function a luminance value between 0.0 and 1.0,
            // and it returns L* which is "perceptual lightness"

            if (luminance <= (216.0 / 24389.0))
                return luminance * (24389.0 / 27.0);

            return Math.Pow(luminance, 1.0 / 3.0) * 116.0 - 16.0;

        }

    }

}
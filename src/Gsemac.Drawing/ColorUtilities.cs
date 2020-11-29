using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public static class ColorUtilities {

        // Public members

        public static double ColorDistance(Color first, Color second, IColorDistanceAlgorithm algorithm = null) {

            if (algorithm is null)
                algorithm = new DeltaEColorDistanceAlgorithm();

            return algorithm.GetDistance(first, second, normalize: true);

        }

        public static Color ToGreyscale(Color color) {

            int greyColor = (int)((0.11 * color.R) + (0.59 * color.G) + (0.30 * color.B));

            return Color.FromArgb(greyColor, greyColor, greyColor);

        }

    }

}
using Gsemac.Core.Extensions;
using System.Drawing;

namespace Gsemac.Drawing {

    public static class ColorUtilities {

        // Public members

        public static double ComputeSimilarity(Color first, Color second, IColorDistanceStrategy strategy = null) {

            if (strategy is null)
                strategy = new DeltaEColorDistanceStrategy();

            return strategy.ComputeSimilarity(first, second);

        }

        public static Color ToGreyscale(Color color) {

            int greyColor = (int)((0.11 * color.R) + (0.59 * color.G) + (0.30 * color.B));

            return Color.FromArgb(greyColor, greyColor, greyColor);

        }

    }

}
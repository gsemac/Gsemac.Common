using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class GrayscaleRgbDifferenceColorDistanceStrategy :
        IColorDistanceStrategy {

        public double ComputeDistance(Color first, Color second, bool normalizeResult = false) {

            int grayscaleFirst = ColorUtilities.ToGreyscale(first).R;
            int grayscaleSecond = ColorUtilities.ToGreyscale(second).R;

            int difference = Math.Abs(grayscaleFirst - grayscaleSecond);

            return normalizeResult ?
                difference / 255.0 :
                difference;

        }

    }

}
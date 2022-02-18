using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class RgbDifferenceStrategy :
        IColorDistanceStrategy {

        public double ComputeDistance(Color first, Color second, bool normalizeResult = false) {

            int rDiff = Math.Abs(first.R - second.R);
            int gDiff = Math.Abs(first.G - second.G);
            int bDiff = Math.Abs(first.B - second.B);

            int difference = rDiff + gDiff + bDiff;

            return normalizeResult ?
                difference / (255 * 3.0) :
                difference;

        }

    }

}
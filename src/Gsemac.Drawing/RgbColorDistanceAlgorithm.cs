using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class RgbColorDistanceAlgorithm :
        IColorDistanceAlgorithm {

        public double GetDistance(Color first, Color second, bool normalize) {

            int rDist = Math.Abs(first.R - second.R);
            int gDist = Math.Abs(first.G - second.G);
            int bDist = Math.Abs(first.B - second.B);

            return (rDist + gDist + bDist) / (255 * 3.0);

        }

    }

}
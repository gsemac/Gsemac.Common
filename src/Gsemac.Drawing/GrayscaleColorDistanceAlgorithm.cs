using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class GrayscaleColorDistanceAlgorithm :
        IColorDistanceAlgorithm {

        public double GetDistance(Color first, Color second, bool normalize) {

            int grayscaleFirst = ColorUtilities.ToGreyscale(first).R;
            int grayscaleSecond = ColorUtilities.ToGreyscale(second).R;

            return Math.Abs(grayscaleFirst - grayscaleSecond) / 255.0;

        }

    }

}
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class GrayscaleColorDistanceStrategy :
        IColorDistanceStrategy {

        public double ComputeDistance(Color first, Color second, bool normalize) {

            int grayscaleFirst = ColorUtilities.ToGreyscale(first).R;
            int grayscaleSecond = ColorUtilities.ToGreyscale(second).R;

            return Math.Abs(grayscaleFirst - grayscaleSecond) / 255.0;

        }

    }

}
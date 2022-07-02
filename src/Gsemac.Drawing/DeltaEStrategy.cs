using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class DeltaEStrategy :
        IColorDistanceStrategy {

        public double ComputeDistance(Color first, Color second, bool normalizeResult = false) {

            // Algorithm from https://www.easyrgb.com/en/math.php

            LabColor labFirst = ColorUtilities.RgbToLab(first);
            LabColor labSecond = ColorUtilities.RgbToLab(second);

            double deltaE = Math.Sqrt(
                Math.Pow(labFirst.L - labSecond.L, 2) +
                Math.Pow(labFirst.A - labSecond.A, 2) +
                Math.Pow(labFirst.B - labSecond.B, 2)
                );

            if (normalizeResult)
                return Math.Min(deltaE / 100.0, 1.0);
            else
                return deltaE;

        }

    }

}
using Gsemac.Drawing.Extensions;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class DeltaEColorDistanceStrategy :
        IColorDistanceStrategy {

        public double ComputeDistance(Color first, Color second, bool normalizeResult = true) {

            // Algorithm from https://www.easyrgb.com/en/math.php

            LabColor labFirst = first.ToLab();
            LabColor labSecond = second.ToLab();
            
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
using System;

namespace Gsemac.Drawing {

    public struct LabColor {

        // Public members

        public double L => l;
        public double A => a;
        public double B => b;

        public LabColor(double l, double a, double b) {

            this.l = l;
            this.a = a;
            this.b = b;

        }

        public XyzColor ToXyz() {

            // Convert from LAB to XYZ.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double y = (l + 16) / 116.0;
            double x = (a / 500.0) + y;
            double z = y - b / 200.0f;

            x = Math.Pow(x, 3) > 0.008856 ? Math.Pow(x, 3) : (x - 16 / 116.0) / 7.787;
            y = Math.Pow(y, 3) > 0.008856 ? Math.Pow(y, 3) : (y - 16 / 116.0) / 7.787;
            z = Math.Pow(z, 3) > 0.008856 ? Math.Pow(z, 3) : (z - 16 / 116.0) / 7.787;

            x *= 0.95047;
            y *= 1.00000;
            z *= 1.08883;

            return new XyzColor(x, y, z);

        }

        public static LabColor FromXyz(XyzColor color) {

            return FromXyz(color.X, color.Y, color.Z);

        }
        public static LabColor FromXyz(double x, double y, double z) {

            // Convert from XYZ to LAB.
            // Algorithm from https://www.easyrgb.com/en/math.php
            // Using D65 observer values

            x /= referenceX;
            y /= referenceY;
            z /= referenceZ;

            x = x > 0.008856 ? Math.Pow(x, 1 / 3.0) : (7.787 * x) + (16 / 116.0);
            y = y > 0.008856 ? Math.Pow(y, 1 / 3.0) : (7.787 * y) + (16 / 116.0);
            z = z > 0.008856 ? Math.Pow(z, 1 / 3.0) : (7.787 * z) + (16 / 116.0);

            double l = (116 * y) - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new LabColor(l, a, b);

        }

        // Private members

        private const double referenceX = 95.047;
        private const double referenceY = 100.0;
        private const double referenceZ = 108.883;

        private readonly double l;
        private readonly double a;
        private readonly double b;

    }

}
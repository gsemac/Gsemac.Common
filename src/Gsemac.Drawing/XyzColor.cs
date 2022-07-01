using Gsemac.Core;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public struct XyzColor {

        // Public members

        public double X => x;
        public double Y => y;
        public double Z => z;

        public XyzColor(double x, double y, double z) {

            this.x = x;
            this.y = y;
            this.z = z;

        }

        public Color ToRgb() {

            return XyzToRgb(this);

        }

        public static XyzColor FromRgb(Color color) {

            return RgbToXyz(color);

        }

        public override bool Equals(object obj) {

            return GetHashCode() == obj.GetHashCode();

        }
        public override int GetHashCode() {

            return new HashCodeBuilder()
                .Add(X)
                .Add(Y)
                .Add(Z)
                .Build();

        }

        public static bool operator ==(XyzColor left, XyzColor right) {

            return left.Equals(right);

        }
        public static bool operator !=(XyzColor left, XyzColor right) {

            return !(left == right);

        }

        // Private members

        private readonly double x;
        private readonly double y;
        private readonly double z;

        private static XyzColor RgbToXyz(Color color) {

            // Convert from RGB to XYZ.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempR = color.R / 255.0;
            double tempG = color.G / 255.0;
            double tempB = color.B / 255.0;

            tempR = tempR > 0.04045f ? Math.Pow((tempR + 0.055) / 1.055, 2.4) : tempR / 12.92;
            tempG = tempG > 0.04045f ? Math.Pow((tempG + 0.055) / 1.055, 2.4) : tempG / 12.92;
            tempB = tempB > 0.04045f ? Math.Pow((tempB + 0.055) / 1.055, 2.4) : tempB / 12.92;

            tempR *= 100.0;
            tempG *= 100.0;
            tempB *= 100.0;

            double x = tempR * 0.4124 + tempG * 0.3576 + tempB * 0.1805;
            double y = tempR * 0.2126 + tempG * 0.7152 + tempB * 0.0722;
            double z = tempR * 0.0193 + tempG * 0.1192 + tempB * 0.9505;

            return new XyzColor(x, y, z);

        }
        private static Color XyzToRgb(XyzColor color) {

            // Convert from XYZ to RGB.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempX = color.X / 100.0;
            double tempY = color.Y / 100.0;
            double tempZ = color.Z / 100.0;

            double r = tempX * 3.2406 + tempY * -1.5372 + tempZ * -0.4986;
            double g = tempX * -0.9689 + tempY * 1.8758 + tempZ * 0.0415;
            double b = tempX * 0.0557 + tempY * -0.2040 + tempZ * 1.0570;

            r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
            g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
            b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;

            r *= 255.0;
            g *= 255.0;
            b *= 255.0;

            return Color.FromArgb((byte)r, (byte)g, (byte)b);

        }

    }

}
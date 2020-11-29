using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Gsemac.Drawing {

    public class XyzColor {

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

            // Convert from XYZ to RGB.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempX = X / 100.0;
            double tempY = Y / 100.0;
            double tempZ = Z / 100.0;

            double r = tempX * 3.2406 + tempY * -1.5372 + tempZ * -0.4986;
            double g = tempX * -0.9689 + tempY * 1.8758 + tempZ * 0.0415;
            double b = tempX * 0.0557 + tempY * -0.2040 + tempZ * 1.0570;

            r = r > 0.0031308 ?
                1.055 * Math.Pow(r, 1 / 2.4) - 0.055 :
                12.92 * r;

            g = g > 0.0031308 ?
               1.055 * Math.Pow(g, 1 / 2.4) - 0.055 :
               12.92 * g;

            b = b > 0.0031308 ?
              1.055 * Math.Pow(b, 1 / 2.4) - 0.055 :
              12.92 * b;

            r *= 255.0;
            g *= 255.0;
            b *= 255.0;

            return Color.FromArgb((byte)r, (byte)g, (byte)b);

        }

        public static XyzColor FromRgb(Color color) {

            return FromRgb(color.R, color.G, color.B);

        }
        public static XyzColor FromRgb(int r, int g, int b) {

            // Convert from RGB to XYZ.
            // Algorithm from https://www.easyrgb.com/en/math.php

            double tempR = r / 255.0;
            double tempG = g / 255.0;
            double tempB = b / 255.0;

            tempR = tempR > 0.04045f ?
                Math.Pow((tempR + 0.055) / 1.055, 2.4) :
                tempR / 12.92;

            tempG = tempG > 0.04045f ?
                Math.Pow((tempR + 0.055) / 1.055, 2.4) :
                tempG / 12.92;

            tempB = tempB > 0.04045f ?
              Math.Pow((tempR + 0.055) / 1.055, 2.4) :
              tempB / 12.92;

            tempR *= 100.0;
            tempG *= 100.0;
            tempB *= 100.0;

            double x = tempR * 0.4124 + tempG * 0.3576 + tempB * 0.1805;
            double y = tempR * 0.2126 + tempG * 0.7152 + tempB * 0.0722;
            double z = tempR * 0.0193 + tempG * 0.1192 + tempB * 0.9505;

            return new XyzColor(x, y, z);

        }

        // Private members

        private readonly double x;
        private readonly double y;
        private readonly double z;
    }

}
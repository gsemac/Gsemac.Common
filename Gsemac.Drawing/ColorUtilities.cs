using System.Drawing;

namespace Gsemac.Drawing {

    public static class ColorUtilities {

        public static Color Tint(Color baseColor, float factor) {

            // Adapted from the answer given here:
            // https://stackoverflow.com/a/31325812/5383169

            int r = baseColor.R;
            int g = baseColor.G;
            int b = baseColor.B;

            int newR = (int)(r + (255 - r) * factor);
            int newG = (int)(g + (255 - g) * factor);
            int newB = (int)(b + (255 - b) * factor);

            return Color.FromArgb(newR, newG, newB);

        }
        public static Color Shade(Color baseColor, float factor) {

            // Adapted from the answer given here:
            // https://stackoverflow.com/a/31325812/5383169

            factor = 1.0f - factor;

            return Color.FromArgb((int)(baseColor.R * factor), (int)(baseColor.G * factor), (int)(baseColor.B * factor));

        }

    }

}
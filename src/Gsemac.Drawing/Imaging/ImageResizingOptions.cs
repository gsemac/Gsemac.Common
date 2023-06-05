namespace Gsemac.Drawing.Imaging {

    public class ImageResizingOptions :
        IImageResizingOptions {

        // Public members

        public int? Width {
            get => GetWidth();
            set => width = value ?? 0;
        }
        public int? Height {
            get => GetHeight();
            set => height = value ?? 0;
        }
        public double? HorizontalScale {
            get => GetHorizontalScale();
            set => horizontalScale = value ?? 0;
        }
        public double? VerticalScale {
            get => GetVerticalScale();
            set => verticalScale = value ?? 0;
        }
        public ImageSizingMode SizingMode { get; set; } = ImageSizingMode.None;
        public bool MaintainAspectRatio { get; set; } = false;

        public static ImageResizingOptions Default => new ImageResizingOptions();

        // Private members

        private int width;
        private int height;
        private double horizontalScale;
        private double verticalScale;

        private int? GetWidth() {

            if (width <= 0)
                return null;

            return width;

        }
        private int? GetHeight() {

            if (height <= 0)
                return null;

            return height;

        }
        private double? GetHorizontalScale() {

            if (horizontalScale <= 0)
                return null;

            return horizontalScale;

        }
        private double? GetVerticalScale() {

            if (verticalScale <= 0)
                return null;

            return verticalScale;

        }

    }

}
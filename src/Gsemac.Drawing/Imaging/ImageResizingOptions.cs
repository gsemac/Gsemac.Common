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
        public float? HorizontalScale {
            get => GetHorizontalScale();
            set => horizontalScale = value ?? 0;
        }
        public float? VerticalScale {
            get => GetVerticalScale();
            set => verticalScale = value ?? 0;
        }
        public ImageSizingMode SizingMode { get; set; } = ImageSizingMode.None;

        public static ImageResizingOptions Default => new ImageResizingOptions();

        // Private members

        private int width;
        private int height;
        private float horizontalScale;
        private float verticalScale;

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
        private float? GetHorizontalScale() {

            if (horizontalScale <= 0)
                return null;

            return horizontalScale;

        }
        private float? GetVerticalScale() {

            if (verticalScale <= 0)
                return null;

            return verticalScale;

        }

    }

}
namespace Gsemac.Drawing.Imaging {

    public class ImageEncoderOptions :
        IImageEncoderOptions {

        public static ImageEncoderOptions Default => new ImageEncoderOptions();

        public ImageOptimizationMode OptimizationMode { get; set; } = ImageOptimizationMode.None;
        public int Quality { get; set; } = ImageQuality.Best;

        public override bool Equals(object obj) {

            if (obj is IImageEncoderOptions other) {

                return OptimizationMode == other.OptimizationMode &&
                    Quality == other.Quality;

            }
            else {

                return false;

            }

        }
        public override int GetHashCode() {

            return base.GetHashCode();

        }

    }

}
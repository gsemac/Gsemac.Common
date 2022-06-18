namespace Gsemac.Drawing.Imaging {

    public class ImageEncoderOptions :
        IImageEncoderOptions {

        public static ImageEncoderOptions Default => new ImageEncoderOptions();

        public OptimizationMode OptimizationMode { get; set; } = OptimizationMode.None;
        public int Quality { get; set; } = ImageQuality.Best;
        public bool AddFileExtension { get; set; } = false;

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
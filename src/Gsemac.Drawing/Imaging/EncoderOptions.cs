namespace Gsemac.Drawing.Imaging {

    public class EncoderOptions :
        IEncoderOptions {

        public static EncoderOptions Default => new EncoderOptions();

        public OptimizationMode OptimizationMode { get; set; } = OptimizationMode.None;
        public int Quality { get; set; } = ImageQuality.Best;
        public bool AddFileExtension { get; set; } = false;

        public override bool Equals(object obj) {

            if (obj is IEncoderOptions other) {

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
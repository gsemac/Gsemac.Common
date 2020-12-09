namespace Gsemac.Drawing.Imaging {

    public class ImageEncoderOptions :
        IImageEncoderOptions {

        public const int BestQuality = 100;

        public static ImageEncoderOptions Default => new ImageEncoderOptions();

        public ImageCompressionMode CompressionMode { get; set; } = ImageCompressionMode.None;
        public int Quality { get; set; } = BestQuality;

        public override bool Equals(object obj) {

            if (obj is IImageEncoderOptions other) {

                return CompressionMode == other.CompressionMode &&
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
namespace Gsemac.Drawing.Imaging {

    public class ImageEncoderOptions :
        IImageEncoderOptions {

        public const int BestQuality = 100;

        public int Quality { get; set; } = BestQuality;

    }

}
namespace Gsemac.Drawing {

    public class ImageConversionOptions :
        IImageConversionOptions {

        public float Quality { get; set; } = 1.0f;
        public int? Width { get; set; }
        public int? Height { get; set; }

    }

}
namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoderOptions {

        ImageCompressionMode CompressionMode { get; set; }
        int Quality { get; set; }

    }

}
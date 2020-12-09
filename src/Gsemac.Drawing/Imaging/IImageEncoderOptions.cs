namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoderOptions {

        ImageOptimizationMode OptimizationMode { get; set; }
        int Quality { get; set; }

    }

}
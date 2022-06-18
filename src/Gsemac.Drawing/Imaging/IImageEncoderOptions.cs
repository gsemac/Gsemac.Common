namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoderOptions {

        OptimizationMode OptimizationMode { get; }
        int Quality { get; }
        bool AddFileExtension { get; }

    }

}
namespace Gsemac.Drawing.Imaging {

    public interface IEncoderOptions {

        OptimizationMode OptimizationMode { get; }
        int Quality { get; }
        bool AddFileExtension { get; }

    }

}
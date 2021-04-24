using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizerFactory :
        IHasSupportedFileFormats {

        IImageOptimizer FromFileFormat(IFileFormat imageFormat);

    }

}
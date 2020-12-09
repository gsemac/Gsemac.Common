using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizer :
        IHasSupportedImageFormats {

        bool Optimize(Stream stream, ImageOptimizationMode optimizationMode);

    }

}
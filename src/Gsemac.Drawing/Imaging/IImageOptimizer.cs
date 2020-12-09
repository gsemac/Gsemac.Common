using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizer :
        IHasSupportedImageFormats {

        int Priority { get; }

        bool Optimize(Stream stream, ImageOptimizationMode optimizationMode);

    }

}
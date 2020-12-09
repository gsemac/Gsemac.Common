using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizer :
        IHasSupportedImageFormats {

        void Optimize(Stream stream, ImageOptimizationMode optimizationMode);

    }

}
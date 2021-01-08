using Gsemac.IO;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizer :
        IHasSupportedFileFormats {

        bool Optimize(Stream stream, ImageOptimizationMode optimizationMode);

    }

}
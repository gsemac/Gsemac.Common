using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageOptimizer {

        IEnumerable<IImageFormat> SupportedImageFormats { get; }

        void Optimize(Stream stream, ImageOptimizationMode optimizationMode);

    }

}
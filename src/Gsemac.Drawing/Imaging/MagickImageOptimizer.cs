using ImageMagick;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class MagickImageOptimizer :
        IImageOptimizer {

        public IEnumerable<IImageFormat> SupportedImageFormats => new MagickImageCodec().SupportedImageFormats;

        public void Optimize(Stream stream, ImageOptimizationMode optimizationMode) {

            if (optimizationMode != ImageOptimizationMode.None) {

                ImageOptimizer optimizer = new ImageOptimizer {
                    IgnoreUnsupportedFormats = true
                };

                if (optimizationMode == ImageOptimizationMode.Maximum)
                    optimizer.OptimalCompression = true;

                if (optimizationMode == ImageOptimizationMode.Lossless)
                    optimizer.LosslessCompress(stream);
                else
                    optimizer.Compress(stream);

            }

        }

    }

}
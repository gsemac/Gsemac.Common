using Gsemac.Reflection;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblies("Magick.NET-Q16-AnyCPU")]
    [RequiresAssemblyOrType("Magick.NET.Core", "ImageMagick.ImageOptimizer")]
    public class MagickImageOptimizer :
        IImageOptimizer {

        public IEnumerable<IImageFormat> SupportedImageFormats => new MagickImageCodec().SupportedImageFormats;
        public int Priority => 0;

        public bool Optimize(Stream stream, ImageOptimizationMode optimizationMode) {

            if (stream.CanSeek && stream.CanRead) {

                if (optimizationMode != ImageOptimizationMode.None) {

                    ImageMagick.ImageOptimizer optimizer = new ImageMagick.ImageOptimizer {
                        IgnoreUnsupportedFormats = true
                    };

                    if (optimizationMode == ImageOptimizationMode.Maximum)
                        optimizer.OptimalCompression = true;

                    bool result;

                    if (optimizationMode == ImageOptimizationMode.Lossless)
                        result = optimizer.LosslessCompress(stream);
                    else
                        result = optimizer.Compress(stream);

                    return result;

                }

            }

            return false;

        }

    }

}
using Gsemac.IO;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblyOrTypes("Magick.NET-Q16-AnyCPU", "ImageMagick.IImageOptimizer")]
    public class MagickImageOptimizer :
        PluginBase,
        IImageOptimizer {

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return new MagickImageCodec().GetSupportedFileFormats();

        }

        public bool Optimize(Stream stream, OptimizationMode optimizationMode) {

            if (stream.CanSeek && stream.CanRead) {

                if (optimizationMode != OptimizationMode.None) {

                    ImageMagick.ImageOptimizer optimizer = new ImageMagick.ImageOptimizer {
                        IgnoreUnsupportedFormats = true
                    };

                    if (optimizationMode == OptimizationMode.Maximum)
                        optimizer.OptimalCompression = true;

                    bool result;

                    if (optimizationMode == OptimizationMode.Lossless)
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
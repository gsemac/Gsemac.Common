using Gsemac.IO;
using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblyOrType("Magick.NET-Q16-AnyCPU", "ImageMagick.IImageOptimizer")]
    public class MagickImageOptimizer :
        PluginBase,
        IImageOptimizer {

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new MagickImageCodec().GetSupportedFileFormats();

        }

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
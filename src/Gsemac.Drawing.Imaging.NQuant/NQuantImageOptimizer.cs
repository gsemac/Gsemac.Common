#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.Reflection.Plugins;
using nQuant;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    [PluginPriority(Priority.High)]
    [RequiresAssemblyOrTypes("nQuant.Core", "nQuant.WuQuantizer")]
    public class NQuantImageOptimizer :
        PluginBase,
        IImageOptimizer {

        // Public members

        public NQuantImageOptimizer() { }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public bool Optimize(Stream stream, ImageOptimizationMode optimizationMode) {

            if (stream.CanRead && stream.CanSeek) {

                WuQuantizer quantizer = new WuQuantizer();

                using (Bitmap bitmap = (Bitmap)Image.FromStream(stream)) {

                    using (Image quantized = quantizer.QuantizeImage(bitmap)) {

                        stream.Seek(0, SeekOrigin.Begin);
                        stream.SetLength(0);

                        quantized.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    }

                }

                return true;

            }

            return false;

        }

        // Private members

        private IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return new[]{
                ".png"
            }.Select(ext => FileFormatFactory.Default.FromFileExtension(ext));

        }

    }

}

#endif
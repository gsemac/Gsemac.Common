#if NETFRAMEWORK

using nQuant;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class NQuantImageOptimizer :
        IImageOptimizer {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();

        public void Optimize(Stream stream, ImageOptimizationMode optimizationMode) {

            WuQuantizer quantizer = new WuQuantizer();

            using (Bitmap bitmap = (Bitmap)Image.FromStream(stream)) {

                using (Image quantized = quantizer.QuantizeImage(bitmap)) {

                    quantized.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                }

            }

        }

        // Private members

        private IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return new[]{
                ".png"
            }.Select(ext => ImageFormat.FromFileExtension(ext));

        }

    }

}

#endif
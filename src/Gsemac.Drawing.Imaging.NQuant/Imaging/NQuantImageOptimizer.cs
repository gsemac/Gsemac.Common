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
        public int Priority => 1;

        public bool Optimize(Stream stream, ImageOptimizationMode optimizationMode) {

            if (stream.CanRead && stream.CanSeek) {

                WuQuantizer quantizer = new WuQuantizer();

                using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromStream(stream)) {

                    using (System.Drawing.Image quantized = quantizer.QuantizeImage(bitmap)) {

                        stream.Seek(0, SeekOrigin.Begin);

                        quantized.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    }

                }

                return true;

            }

            return false;

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
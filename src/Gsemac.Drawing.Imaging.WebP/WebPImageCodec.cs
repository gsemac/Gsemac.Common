#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WebPWrapper;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblies("libwebp_x86", X86 = true)]
    [RequiresAssemblies("libwebp_x64", X64 = true)]
    [RequiresAssemblyOrTypes("WebPWrapper", "WebPWrapper.WebP")]
    public class WebPImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (Bitmap bitmap = image.ToBitmap())
                EncodeWebPBitmap(bitmap, stream, encoderOptions);

        }
        public IImage Decode(Stream stream) {

            return ImageUtilities.CreateImageFromBitmap(DecodeWebPBitmap(stream), GetSupportedFileFormats().First(), this);

        }

        // Private members

        private IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return new[]{
                ".webp"
            }.Select(ext => FileFormatFactory.Default.FromFileExtension(ext));

        }

        private void EncodeWebPBitmap(Bitmap bitmap, Stream stream, IImageEncoderOptions encoderOptions) {

            bool useLosslessEncoding = encoderOptions.Quality >= 100 ||
                encoderOptions.OptimizationMode == ImageOptimizationMode.Lossless;

            using (WebP encoder = new WebP())
            using (MemoryStream webPStream = new MemoryStream(useLosslessEncoding ? encoder.EncodeLossless(bitmap) : encoder.EncodeLossy(bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        private Image DecodeWebPBitmap(Stream stream) {

            using (WebP decoder = new WebP())
                return decoder.Decode(stream.ToArray());

        }

    }

}

#endif
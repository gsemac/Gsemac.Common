#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblies("libwebp_x86", X86 = true)]
    [RequiresAssemblies("libwebp_x64", X64 = true)]
    [RequiresAssemblyOrType("WebPWrapper", "WebPWrapper.WebP")]
    public class WebPImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => GetSupportedImageFormats();

        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (Bitmap bitmap = image.ToBitmap())
                EncodeWebPBitmap(bitmap, stream, encoderOptions);

        }
        public IImage Decode(Stream stream) {

            return Image.FromBitmap(DecodeWebPBitmap(stream), SupportedFileFormats.First(), this);

        }

        // Private members

        private IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return new[]{
                ".webp"
            }.Select(ext => FileFormat.FromFileExtension(ext));

        }

        private void EncodeWebPBitmap(System.Drawing.Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (WebPWrapper.WebP encoder = new WebPWrapper.WebP())
            using (MemoryStream webPStream = new MemoryStream(encoder.EncodeLossy(image as Bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        private System.Drawing.Image DecodeWebPBitmap(Stream stream) {

            using (WebPWrapper.WebP decoder = new WebPWrapper.WebP())
                return decoder.Decode(stream.ToArray());

        }

    }

}

#endif
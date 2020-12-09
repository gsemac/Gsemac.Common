#if NETFRAMEWORK

using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class WebPImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();
        public int Priority => 0;

        public System.Drawing.Image Decode(Stream stream) {

            using (WebPWrapper.WebP decoder = new WebPWrapper.WebP())
                return decoder.Decode(stream.ToArray());

        }
        IImage IImageDecoder.Decode(Stream stream) {

            return new GdiImage(Decode(stream), this);

        }
        public void Encode(System.Drawing.Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (WebPWrapper.WebP encoder = new WebPWrapper.WebP())
            using (MemoryStream webPStream = new MemoryStream(encoder.EncodeLossy(image as Bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (Bitmap bitmap = image.ToBitmap())
                Encode(bitmap, stream, encoderOptions);

        }

        // Private members

        private IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return new[]{
                ".webp"
            }.Select(ext => ImageFormat.FromFileExtension(ext));

        }

    }

}

#endif
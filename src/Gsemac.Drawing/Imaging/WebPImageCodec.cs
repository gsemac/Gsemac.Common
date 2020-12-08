#if NETFRAMEWORK

using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class WebPImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<string> SupportedFileTypes => GetSupportedFileTypes();

        public Image Decode(Stream stream) {

            using (WebPWrapper.WebP decoder = new WebPWrapper.WebP())
                return decoder.Decode(stream.ToArray());

        }
        IImage IImageDecoder.Decode(Stream stream) {

            return new GdiImage(Decode(stream));

        }
        public void Encode(Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (WebPWrapper.WebP encoder = new WebPWrapper.WebP())
            using (MemoryStream webPStream = new MemoryStream(encoder.EncodeLossy(image as Bitmap, encoderOptions.Quality)))
                webPStream.CopyTo(stream);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (Bitmap bitmap = image.ToBitmap())
                Encode(bitmap, stream, encoderOptions);

        }

        // Private members

        private IEnumerable<string> GetSupportedFileTypes() {

            List<string> extensions = new List<string>(new[]{
                ".webp"
            });

            return extensions;

        }

    }

}

#endif
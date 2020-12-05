#if NETFRAMEWORK

using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class WebPImageReader :
        IImageReader {

        // Public members

        public IEnumerable<string> SupportedFileTypes => GetSupportedFileTypes();

        public Image ReadImage(Stream stream) {

            using (WebPWrapper.WebP decoder = new WebPWrapper.WebP())
                return decoder.Decode(stream.ToArray());

        }
        public void SaveImage(Image image, Stream stream, IImageEncoderOptions options) {

            using (WebPWrapper.WebP encoder = new WebPWrapper.WebP())
            using (MemoryStream webPStream = new MemoryStream(encoder.EncodeLossy(image as Bitmap, options.Quality)))
                webPStream.CopyTo(stream);

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
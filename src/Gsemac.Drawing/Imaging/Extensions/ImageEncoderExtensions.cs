using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageEncoderExtensions {

#if NETFRAMEWORK

        public static void Encode(this IImageEncoder encoder, Image image, Stream stream, IImageEncoderOptions options) {

            encoder.Encode(new GdiImage(image), stream, options);

        }

#endif

    }

}
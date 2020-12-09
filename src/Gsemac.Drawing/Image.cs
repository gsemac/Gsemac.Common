using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;

namespace Gsemac.Drawing {

    public static class Image {

        public static IImage FromFile(string filePath) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw ImageExceptions.UnsupportedImageFormat;

            return imageCodec.Decode(filePath);

        }

    }

}
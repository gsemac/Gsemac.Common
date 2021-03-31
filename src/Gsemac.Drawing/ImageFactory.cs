using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;

namespace Gsemac.Drawing {

    public class ImageFactory :
        IImageFactory {

        public IImage FromFile(string filePath) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw new UnsupportedFileFormatException();

            return imageCodec.Decode(filePath);

        }

    }

}
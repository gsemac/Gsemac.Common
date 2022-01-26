using Gsemac.Drawing.Imaging;

#if NETFRAMEWORK
using Gsemac.IO;
using System.Drawing;
#endif

namespace Gsemac.Drawing {

    public class ImageFactory :
        ImageFactoryBase {

        // Public members

        public static ImageFactory Default => new ImageFactory();

        public ImageFactory() :
            this(ImageCodecFactory.Default) {
        }
        public ImageFactory(IImageCodecFactory imageCodecFactory) :
            base(imageCodecFactory) {
        }

#if NETFRAMEWORK
        public static IImage FromBitmap(Image bitmap) {

            return FromBitmap(bitmap, null, null);

        }
        public static IImage FromBitmap(Image bitmap, IFileFormat imageFormat, IImageCodec imageCodec) {

            return new GdiImage(bitmap, imageFormat, imageCodec);

        }
#endif

    }

}
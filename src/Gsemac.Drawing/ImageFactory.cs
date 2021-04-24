using Gsemac.Drawing.Imaging;

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

    }

}
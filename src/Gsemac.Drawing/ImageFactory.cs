using System;

namespace Gsemac.Drawing {

    public class ImageFactory :
        ImageFactoryBase {

        // Public members

        public static ImageFactory Default => new ImageFactory();

        public ImageFactory() { }
        public ImageFactory(IServiceProvider serviceProvider) :
            base(serviceProvider) {
        }

    }

}
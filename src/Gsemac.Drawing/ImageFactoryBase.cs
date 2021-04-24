using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.Drawing {

    public abstract class ImageFactoryBase :
        IImageFactory {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => imageCodecFactory.SupportedFileFormats;

        public IImage FromFile(string filePath) {

            IImageCodec imageCodec = imageCodecFactory.FromFileExtension(filePath);

            if (imageCodec is null)
                throw new UnsupportedFileFormatException();

            return imageCodec.Decode(filePath);

        }

        // Protected members

        protected ImageFactoryBase(IImageCodecFactory imageCodecFactory) {

            this.imageCodecFactory = imageCodecFactory;

        }

        // Private members

        private readonly IImageCodecFactory imageCodecFactory;

    }

}
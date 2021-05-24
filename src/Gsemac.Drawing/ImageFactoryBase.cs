using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing {

    public abstract class ImageFactoryBase :
        IImageFactory {

        // Public members

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return imageCodecFactory.GetSupportedFileFormats();

        }

        public IImage FromStream(Stream stream, IFileFormat imageFormat = null) {

            if (imageFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out imageFormat);

            IImageCodec imageCodec = imageCodecFactory.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return imageCodec.Decode(stream);

        }

        // Protected members

        protected ImageFactoryBase(IImageCodecFactory imageCodecFactory) {

            this.imageCodecFactory = imageCodecFactory;

        }

        // Private members

        private readonly IImageCodecFactory imageCodecFactory;

    }

}
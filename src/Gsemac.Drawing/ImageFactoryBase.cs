using Gsemac.Drawing.Imaging;
using Gsemac.IO;
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

            if (imageFormat is null) {

                imageFormat = FileFormatFactory.Default.FromStream(stream);

                stream.Seek(0, SeekOrigin.Begin);

            }

            IImageCodec imageCodec = imageCodecFactory.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new UnsupportedFileFormatException();

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
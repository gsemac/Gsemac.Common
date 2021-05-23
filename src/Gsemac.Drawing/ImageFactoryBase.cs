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

                // Read the file signature from the stream into a buffer, which is then concatenated with the original stream.
                // This allows us to read the file signature from streams that don't support seeking.

                const int maxSignatureLength = 64;

                using (MemoryStream fileSignatureBuffer = new MemoryStream(new byte[maxSignatureLength], 0, maxSignatureLength, writable: true, publiclyVisible: true)) {

                    int bytesRead = stream.Read(fileSignatureBuffer.GetBuffer(), 0, maxSignatureLength);

                    fileSignatureBuffer.SetLength(bytesRead);

                    imageFormat = FileFormatFactory.Default.FromStream(fileSignatureBuffer);

                    fileSignatureBuffer.Seek(0, SeekOrigin.Begin);

                    return FromStream(new ConcatStream(fileSignatureBuffer, stream), imageFormat);

                }

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
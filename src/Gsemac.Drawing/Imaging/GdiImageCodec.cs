#if NETFRAMEWORK

using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class GdiImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<string> SupportedFileTypes => ImageCodec.NativelySupportedFileTypes;

        public GdiImageCodec() {
        }
        public GdiImageCodec(IImageFormat imageFormat) {

            if (!this.IsSupportedFileType(imageFormat.FileExtension))
                throw new FileFormatException("The image format is not supported.");

            this.imageFormat = imageFormat;

        }

        public Image Decode(Stream stream) {

            // Image requires that the stream be kept open, because it is read lazily.
            // By creating a new Bitmap from the image, we force it to read the stream immediately.

            return new Bitmap(Image.FromStream(stream));

        }
        IImage IImageDecoder.Decode(Stream stream) {

            return new GdiImage(Decode(stream));

        }
        public void Encode(Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            Encode(image, stream, imageFormat, encoderOptions);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            Encode(image, stream, imageFormat, encoderOptions);

        }

        // Private members

        private readonly IImageFormat imageFormat;

        private void Encode(Image image, Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            new GdiImage(image).Save(stream, imageFormat, encoderOptions);

        }
        private void Encode(IImage image, Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            image.Save(stream, imageFormat, encoderOptions);

        }

    }

}

#endif
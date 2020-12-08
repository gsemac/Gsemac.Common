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

            using (Image imageFromStream = Image.FromStream(stream))
                return new Bitmap(imageFromStream);

        }
        IImage IImageDecoder.Decode(Stream stream) {

            // When we create a new Bitmap from the Image, we lose information about its original format (it just becomes a memory Bitmap).
            // This GdiImage constructor allows us to preserve the original format information.

            using (Image imageFromStream = Image.FromStream(stream))
                return new GdiImage(new Bitmap(imageFromStream), imageFromStream.RawFormat);

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

            if (image is GdiImage gdiImage) {

                // If the image is aleady a GdiImage, we can save it directly.

                if (imageFormat is null)
                    gdiImage.Save(stream);
                else
                    gdiImage.Save(stream, imageFormat, encoderOptions);

            }
            else {

                // If the image is not a GdiImage, save it to an intermediate stream and load it.

                using (MemoryStream ms = new MemoryStream()) {

                    image.Save(ms);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (gdiImage = new GdiImage(new Bitmap(ms)))
                        gdiImage.Save(stream, imageFormat, encoderOptions);

                }

            }

        }

    }

}

#endif
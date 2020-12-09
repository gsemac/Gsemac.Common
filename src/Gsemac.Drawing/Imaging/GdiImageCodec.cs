#if NETFRAMEWORK

using Gsemac.Drawing.Imaging.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class GdiImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => ImageCodec.NativelySupportedImageFormats;

        public GdiImageCodec() {
        }
        public GdiImageCodec(IImageFormat imageFormat) {

            if (!this.IsSupportedImageFormat(imageFormat.FileExtension))
                throw ImageExceptions.UnsupportedImageFormat;

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

                // If the image is not a GdiImage, convert it to a bitmap and load it.

                using (Bitmap intermediateBitmap = image.ToBitmap())
                using (gdiImage = new GdiImage(intermediateBitmap))
                    gdiImage.Save(stream, imageFormat, encoderOptions);

            }

        }

    }

}

#endif
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using ImageMagick;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public class MagickImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<string> SupportedFileTypes => GetSupportedFileTypes();

        public MagickImageCodec() {
        }
        public MagickImageCodec(IImageFormat imageFormat) {

            if (!this.IsSupportedFileType(imageFormat.FileExtension))
                throw new FileFormatException("The image format is not supported.");

            this.imageFormat = imageFormat;

        }

        public IImage Decode(Stream stream) {

            return new MagickImage(new ImageMagick.MagickImage(stream));

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is MagickImage magickImage) {

                // If the image is aleady a MagickImage, we can save it directly.

                if (imageFormat is null)
                    magickImage.Save(stream);
                else
                    magickImage.Save(stream, imageFormat, encoderOptions);

            }
            else {

                // If the image is not a MagickImage, save it to an intermediate stream and load it.

                using (MemoryStream ms = new MemoryStream()) {

                    image.Save(ms);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (magickImage = new MagickImage(new ImageMagick.MagickImage(ms)))
                        magickImage.Save(stream, imageFormat, encoderOptions);

                }

            }

        }

        // Private members

        private readonly IImageFormat imageFormat;

        private static IEnumerable<string> GetSupportedFileTypes() {

            return new[] {
                ".avif",
                ".bmp",
                ".gif",
                ".jpg",
                ".jpeg",
                ".png",
                ".tif",
                ".tiff",
                ".webp",
            };

        }

    }

}
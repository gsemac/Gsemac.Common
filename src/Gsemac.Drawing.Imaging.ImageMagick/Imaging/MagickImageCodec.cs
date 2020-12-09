using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;
using ImageMagick;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class MagickImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();
        public int Priority => 1;

        public MagickImageCodec() {
        }
        public MagickImageCodec(IImageFormat imageFormat) {

            if (!this.IsSupportedImageFormat(imageFormat))
                throw ImageExceptions.UnsupportedImageFormat;

            this.imageFormat = imageFormat;

        }

        public IImage Decode(Stream stream) {

            return new MagickImage(new ImageMagick.MagickImage(stream), this);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is MagickImage magickImage) {

                // If the image is aleady a MagickImage, we can save it directly.

                Save(magickImage.BaseImage, stream, encoderOptions);

            }
            else {

                // If the image is not a MagickImage, save it to an intermediate stream and load it.

                using (MemoryStream ms = new MemoryStream()) {

                    image.Codec.Encode(image, ms);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (magickImage = new MagickImage(new ImageMagick.MagickImage(ms), this)) {
                        Save(magickImage.BaseImage, stream, encoderOptions);

                    }

                }

            }

        }

        // Private members

        private readonly IImageFormat imageFormat;

        private static IEnumerable<IImageFormat> GetSupportedImageFormats() {

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
            }.OrderBy(ext => ext)
            .Select(ext => ImageFormat.FromFileExtension(ext))
            .Distinct(new ImageFormatComparer());

        }
        private static MagickFormat GetMagickFormatForFileExtension(string fileExtension) {

            switch (fileExtension.ToLowerInvariant()) {

                case ".avif":
                    return MagickFormat.Avif;

                case ".bmp":
                    return MagickFormat.Bmp;

                case ".gif":
                    return MagickFormat.Gif;

                case ".jpg":
                    return MagickFormat.Jpg;

                case ".jpeg":
                    return MagickFormat.Jpeg;

                case ".png":
                    return MagickFormat.Png;

                case ".tif":
                    return MagickFormat.Tif;

                case ".tiff":
                    return MagickFormat.Tiff;

                case ".webp":
                    return MagickFormat.WebP;

                default:
                    throw ImageExceptions.UnsupportedImageFormat;

            }

        }

        private void Save(ImageMagick.MagickImage magickImage, Stream stream, IImageEncoderOptions encoderOptions) {

            if (!(imageFormat is null))
                magickImage.Format = GetMagickFormatForFileExtension(imageFormat.FileExtension);

            magickImage.Quality = encoderOptions.Quality;

            magickImage.Write(stream);

        }

    }

}
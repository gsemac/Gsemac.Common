using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    [PluginPriority(Priority.High)]
    [RequiresAssemblyOrTypes("Magick.NET.Core", "ImageMagick.IMagickImage")]
    [RequiresAssemblyOrTypes("Magick.NET-Q16-AnyCPU", "ImageMagick.MagickImage")]
#if NETFRAMEWORK
    [RequiresAssemblies("Magick.NET.SystemDrawing")]
#endif
    public class MagickImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public MagickImageCodec() { }
        public MagickImageCodec(IFileFormat imageFormat) {

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (!this.IsSupportedFileFormat(imageFormat))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            this.imageFormat = imageFormat;

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImage Decode(Stream stream, IImageDecoderOptions options) {

            return new MagickImage(stream, imageFormat, this);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is MagickImage magickImage) {

                // If the image is aleady a MagickImage, we can save it directly.

                Save(magickImage.BaseImage, stream, encoderOptions);

            }
            else {

                // If the image is not a MagickImage, save it to an intermediate stream and load it.

                // If the image's codec is an instance of MagickImageCodec but it isn't a MagickImage instance, the codec is mismatched.
                // This shouldn't happen under normal conditions other than user error, but it will result in infinite recursion when calling Encode.

                if (image.Codec is MagickImageCodec)
                    throw new CodecMismatchException();

                using (MemoryStream ms = new MemoryStream()) {

                    image.Codec.Encode(image, ms);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (magickImage = new MagickImage(ms, imageFormat, this))
                        Save(magickImage.BaseImage, stream, encoderOptions);

                }

            }

        }

        // Private members

        private readonly IFileFormat imageFormat;

        private static IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return new[] {
                ".avif",
                ".bmp",
                ".gif",
                ".ico",
                ".jpeg",
                ".jpg",
                ".jxl",
                ".png",
                ".svg",
                ".tif",
                ".tiff",
                ".webp",
            }.OrderBy(ext => ext)
            .Select(ext => FileFormatFactory.Default.FromFileExtension(ext))
            .Distinct();

        }
        private static MagickFormat GetMagickFormatForFileExtension(string fileExtension) {

            MagickFormat? key = ImageMagickUtilities.GetMagickFormatFromFileExtension(fileExtension.ToLowerInvariant());

            if (!key.HasValue)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return key.Value;

        }

        private void Save(IMagickImage magickImage, Stream stream, IImageEncoderOptions encoderOptions) {

            if (imageFormat is object)
                magickImage.Format = GetMagickFormatForFileExtension(imageFormat.Extensions.FirstOrDefault());

            magickImage.Quality = encoderOptions.Quality;

            magickImage.Write(stream);

        }

    }

}
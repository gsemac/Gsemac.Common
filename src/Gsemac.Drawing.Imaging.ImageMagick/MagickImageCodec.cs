﻿using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using ImageMagick;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    [RequiresAssemblyOrType("Magick.NET.Core", "ImageMagick.IMagickImage")]
    [RequiresAssemblyOrType("Magick.NET-Q16-AnyCPU", "ImageMagick.MagickImage")]
    public class MagickImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();

        public MagickImageCodec() :
            base(1) {
        }
        public MagickImageCodec(IImageFormat imageFormat) :
            base(1) {

            if (!this.IsSupportedImageFormat(imageFormat))
                throw new ImageFormatException();

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

                    using (magickImage = new MagickImage(new ImageMagick.MagickImage(ms), this))
                        Save(magickImage.BaseImage, stream, encoderOptions);

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

            MagickFormat? key = ImageMagickUtilities.GetMagickFormatFromFileExtension(fileExtension.ToLowerInvariant());

            if (!key.HasValue)
                throw new ImageFormatException();

            return key.Value;

        }

        private void Save(ImageMagick.MagickImage magickImage, Stream stream, IImageEncoderOptions encoderOptions) {

            if (!(imageFormat is null))
                magickImage.Format = GetMagickFormatForFileExtension(imageFormat.FileExtension);

            magickImage.Quality = encoderOptions.Quality;

            magickImage.Write(stream);

        }

    }

}
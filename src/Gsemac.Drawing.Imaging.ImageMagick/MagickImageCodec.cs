﻿using Gsemac.Drawing.Imaging.Extensions;
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

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImage Decode(Stream stream, IImageDecoderOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return new MagickImage(stream, imageFormat, this, options);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            if (image is MagickImage magickImage) {

                // If the image is aleady a MagickImage, we can save it directly.

                Save(magickImage.BaseImages, stream, encoderOptions);

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

                    using (magickImage = new MagickImage(ms, image.Format, this))
                        Save(magickImage.BaseImages, stream, encoderOptions);

                }

            }

        }

        // Private members

        private readonly IFileFormat imageFormat;

        private static IEnumerable<ICodecCapabilities> GetSupportedImageFormats() {

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
            .Distinct()
            .Select(format => new CodecCapabilities(format, canRead: true, canWrite: true));

        }
        private static MagickFormat GetMagickFormatForFileExtension(string fileExtension) {

            IFileFormat fileFormat = FileFormatFactory.Default.FromFileExtension(fileExtension);
            MagickFormat magickFormat = ImageMagickUtilities.GetMagickFormatFromFileFormat(fileFormat);

            if (magickFormat == MagickFormat.Unknown)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return magickFormat;

        }

        private void Save(MagickImageCollection images, Stream stream, IImageEncoderOptions options) {

            if (images is null)
                throw new ArgumentNullException(nameof(images));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            MagickFormat outputFormat = imageFormat is object ?
                GetMagickFormatForFileExtension(imageFormat.Extensions.FirstOrDefault()) :
                MagickFormat.Unknown;

            foreach (IMagickImage image in images) {

#if NETFRAMEWORK

                // Magick.NET-Q16-AnyCPU <= 8.6.1 (and possibly newer) has a bug where encoding JXL images with quality != 100 causes a memory access violation.
                // The following issue might be related: https://github.com/dlemstra/Magick.NET/issues/1130
                // We want to avoid modifying the image quality for JXL images.

                if (outputFormat == MagickFormat.Jxl) {

                    image.Quality = 100;

                    continue;

                }

#endif

                image.Quality = options.Quality;

            }

            if (outputFormat != MagickFormat.Unknown) {

                images.Write(stream, outputFormat);

            }
            else {

                images.Write(stream);

            }

        }

    }

}
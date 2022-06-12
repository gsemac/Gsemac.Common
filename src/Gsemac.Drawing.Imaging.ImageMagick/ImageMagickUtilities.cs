using Gsemac.Collections.Extensions;
using Gsemac.IO;
using Gsemac.IO.FileFormats;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    internal static class ImageMagickUtilities {

        // Public members

        public static MagickFormat GetMagickFormatFromFileFormat(IFileFormat fileFormat) {

            if (fileFormat is null)
                return MagickFormat.Unknown;

            if (formatDictionary.Value.Reverse().TryGetValue(fileFormat, out MagickFormat magickFormat))
                return magickFormat;

            return MagickFormat.Unknown;

        }
        public static IFileFormat GetFileFormatFromMagickFormat(MagickFormat magickFormat) {

            if (formatDictionary.Value.TryGetValue(magickFormat, out IFileFormat fileFormat))
                return fileFormat;

            return null;

        }

        // Private members

        private static readonly Lazy<IDictionary<MagickFormat, IFileFormat>> formatDictionary = new Lazy<IDictionary<MagickFormat, IFileFormat>>(CreateFormatDictionary);

        private static IDictionary<MagickFormat, IFileFormat> CreateFormatDictionary() {

            return new Dictionary<MagickFormat, IFileFormat>{
                { MagickFormat.Avif, ImageFormat.Avif },
                { MagickFormat.Bmp, ImageFormat.Bmp },
                { MagickFormat.Gif, ImageFormat.Gif },
                { MagickFormat.Ico, ImageFormat.Ico },
                { MagickFormat.Jpeg, ImageFormat.Jpeg },
                { MagickFormat.Jpg, ImageFormat.Jpeg },
                { MagickFormat.Jxl, ImageFormat.Jxl },
                { MagickFormat.Png, ImageFormat.Png },
                { MagickFormat.Tif, ImageFormat.Tiff },
                { MagickFormat.Tiff, ImageFormat.Tiff },
                { MagickFormat.WebP, ImageFormat.WebP },
                { MagickFormat.Wmf, ImageFormat.Wmf },
            };

        }

    }

}
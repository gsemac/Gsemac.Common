using Gsemac.IO;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    internal static class ImageMagickUtilities {

        // Public members

        public static MagickFormat GetMagickFormatFromFileExtension(string value) {

            if (!string.IsNullOrEmpty(value)) {

                IEnumerable<MagickFormat> formats = dict.Value
                    .Where(pair => pair.Value == value)
                    .Select(pair => pair.Key);

                if (formats.Any())
                    return formats.First();

            }

            return MagickFormat.Unknown;

        }
        public static MagickFormat GetMagickFormatFromFileFormat(IFileFormat fileFormat) {

            string fileExtension = fileFormat?.Extensions?.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(fileExtension))
                return GetMagickFormatFromFileExtension(fileExtension);

            return MagickFormat.Unknown;

        }
        public static string GetFileExtensionFromMagickFormat(MagickFormat magickFormat) {

            if (dict.Value.TryGetValue(magickFormat, out string value))
                return value;

            return string.Empty;

        }

        public static IEnumerable<MagickFormat> GetMagickFormats() {

            return dict.Value.Keys;

        }
        public static IEnumerable<string> GetFileExtensions() {

            return dict.Value.Values;

        }

        // Private members

        private static readonly Lazy<IDictionary<MagickFormat, string>> dict = new Lazy<IDictionary<MagickFormat, string>>(CreateDict);

        private static IDictionary<MagickFormat, string> CreateDict() {

            return new Dictionary<MagickFormat, string>{
                { MagickFormat.Avif, ".avif" },
                { MagickFormat.Bmp, ".bmp" },
                { MagickFormat.Gif, ".gif" },
                { MagickFormat.Ico, ".ico" },
                { MagickFormat.Jpeg, ".jpeg" },
                { MagickFormat.Jpg, ".jpg" },
                { MagickFormat.Jxl, ".jxl" },
                { MagickFormat.Png, ".png" },
                { MagickFormat.Tif, ".tif" },
                { MagickFormat.Tiff, ".tiff" },
                { MagickFormat.WebP, ".webp" },
                { MagickFormat.Wmf, ".wmf" },
            };

        }

    }

}
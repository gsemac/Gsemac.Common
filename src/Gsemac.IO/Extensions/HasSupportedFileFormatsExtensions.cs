using System;
using System.Linq;

namespace Gsemac.IO.Extensions {

    public static class HasSupportedFileFormatsExtensions {

        public static bool IsSupportedFileFormat(this IHasSupportedFileFormats hasSupportedFileFormats, string filePath) {

            if (hasSupportedFileFormats is null)
                throw new ArgumentNullException(nameof(hasSupportedFileFormats));

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            return hasSupportedFileFormats.IsSupportedFileFormat(FileFormatFactory.Default.FromFileExtension(ext));

        }
        public static bool IsSupportedFileFormat(this IHasSupportedFileFormats hasSupportedFileFormats, IFileFormat fileFormat) {

            if (hasSupportedFileFormats is null)
                throw new ArgumentNullException(nameof(hasSupportedFileFormats));

            return hasSupportedFileFormats.GetSupportedFileFormats().Any(supportedFileFormat => supportedFileFormat.Format.Equals(fileFormat));

        }

    }

}
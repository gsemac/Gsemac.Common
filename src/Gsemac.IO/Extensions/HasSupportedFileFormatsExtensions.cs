using System.Linq;

namespace Gsemac.IO.Extensions {

    public static class HasSupportedFileFormatsExtensions {

        public static bool IsSupportedFileFormat(this IHasSupportedFileFormats obj, string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            return obj.IsSupportedFileFormat(FileFormatFactory.Default.FromFileExtension(ext));

        }
        public static bool IsSupportedFileFormat(this IHasSupportedFileFormats obj, IFileFormat fileFormat) {

            return obj.SupportedFileFormats.Any(supportedFileFormat => supportedFileFormat.Equals(fileFormat));

        }

    }

}
using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class ArchiveDecoder {

        // Public members

        public static IEnumerable<IFileFormat> SupportedFileFormats => GetSupportedArchiveFormats();

        public static bool IsSupportedFileFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            return IsSupportedFileFormat(FileFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedFileFormat(IFileFormat fileFormat) {

            return SupportedFileFormats.Any(format => format.Equals(fileFormat));

        }

        public static IArchiveDecoder FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return FromFileFormat(FileFormat.FromFileExtension(ext));

        }
        public static IArchiveDecoder FromFileFormat(IFileFormat fileFormat) {

            return CompressionPluginLoader.GetArchiveDecoders().FirstOrDefault(decoder => decoder.IsSupportedFileFormat(fileFormat));

        }

        // Private members

        private static IEnumerable<IFileFormat> GetSupportedArchiveFormats() {

            return CompressionPluginLoader.GetArchiveDecoders().SelectMany(decoder => decoder.SupportedFileFormats)
                .OrderBy(type => type)
                .Distinct();

        }

    }

}
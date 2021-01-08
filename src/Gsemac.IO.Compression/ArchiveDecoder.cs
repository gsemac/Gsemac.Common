using Gsemac.IO.Extensions;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class ArchiveDecoder {

        public static IArchiveDecoder FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return FromFileFormat(FileFormat.FromFileExtension(ext));

        }
        public static IArchiveDecoder FromFileFormat(IFileFormat fileFormat) {

            return CompressionPluginLoader.GetArchiveDecoders().FirstOrDefault(decoder => decoder.IsSupportedFileFormat(fileFormat));

        }

    }

}
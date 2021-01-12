using Gsemac.IO.Compression.Extensions;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class Archive {

        // Public members

        public static IArchive Open(string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            IArchiveDecoder decoder = ArchiveDecoder.FromFileExtension(filePath);

            if (decoder is null)
                throw new UnsupportedFileFormatException();

            return decoder.DecodeFile(filePath, fileAccess, options);

        }
        public static IArchive Open(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            // #todo Detect the type of the archive from the first bytes of the stream.

            return CompressionPluginLoader.GetArchiveDecoders().First()
                .Decode(stream, fileAccess, leaveOpen, options);
        }
        public static IArchive OpenRead(string filePath) {

            return Open(filePath, FileAccess.Read);

        }

        public static void Extract(string filePath, bool extractToNewFolder = true) {

            string outputPath = Path.GetDirectoryName(filePath);

            if (extractToNewFolder)
                outputPath = Path.Combine(outputPath, PathUtilities.GetFileNameWithoutExtension(filePath));

            Extract(filePath, outputPath);

        }
        public static void Extract(string filePath, string directoryPath) {

            using (IArchive archive = Open(filePath, FileAccess.Read))
                archive.ExtractAllEntries(directoryPath);

        }

    }

}
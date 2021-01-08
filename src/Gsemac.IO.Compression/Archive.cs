using Gsemac.IO.Compression.Extensions;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public static class Archive {

        // Public members

        public static IArchive OpenFile(string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            IArchiveDecoder decoder = ArchiveDecoder.FromFileExtension(filePath);

            if (decoder is null)
                throw new UnsupportedFileFormatException();

            return decoder.DecodeFile(filePath, fileAccess, options);

        }
        public static IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            // #todo Detect the type of the archive from the first bytes of the stream.

            return CompressionPluginLoader.GetArchiveDecoders().First()
                .Decode(stream, fileAccess, leaveOpen, options);
        }

        public static void Extract(string filePath, string directoryPath) {

            using (IArchive archive = OpenFile(filePath, FileAccess.Read))
                archive.ExtractAllEntries(directoryPath);

        }

    }

}
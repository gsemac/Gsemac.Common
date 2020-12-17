using System.IO;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveReaderExtensions {

        public static IArchive OpenFile(this IArchiveReader archiveReader, string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            return archiveReader.OpenStream(new FileStream(filePath, FileMode.OpenOrCreate, fileAccess), fileAccess, leaveOpen: false, options);

        }

    }

}
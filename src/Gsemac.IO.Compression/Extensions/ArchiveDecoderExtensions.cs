using System.IO;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveDecoderExtensions {

        public static IArchive DecodeFile(this IArchiveDecoder archiveReader, string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            return archiveReader.Decode(new FileStream(filePath, FileMode.OpenOrCreate, fileAccess), fileAccess, leaveOpen: false, options);

        }

    }

}
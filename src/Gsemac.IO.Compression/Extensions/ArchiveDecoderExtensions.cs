using System.IO;

namespace Gsemac.IO.Compression.Extensions {

    public static class ArchiveDecoderExtensions {

        public static IArchive DecodeFile(this IArchiveDecoder archiveReader, string filePath, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) {

            FileMode fileMode = fileAccess == FileAccess.Read ?
                FileMode.Open :
                FileMode.OpenOrCreate;

            return archiveReader.Decode(new FileStream(filePath, fileMode, fileAccess), fileAccess, leaveOpen: false, options);

        }

    }

}
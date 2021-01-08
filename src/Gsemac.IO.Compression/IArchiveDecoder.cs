using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchiveDecoder :
        IHasSupportedFileFormats {

        IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null);

    }

}
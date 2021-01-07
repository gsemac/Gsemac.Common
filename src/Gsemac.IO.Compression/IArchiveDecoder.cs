using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchiveDecoder {

        IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null);

    }

}
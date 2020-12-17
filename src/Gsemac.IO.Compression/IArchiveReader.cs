using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchiveReader {

        IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null);

    }

}
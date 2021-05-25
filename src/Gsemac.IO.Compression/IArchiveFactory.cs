using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchiveFactory :
        IHasSupportedFileFormats {

        IArchive Open(Stream stream, IFileFormat archiveFormat = null, IArchiveOptions archiveOptions = null);

    }

}
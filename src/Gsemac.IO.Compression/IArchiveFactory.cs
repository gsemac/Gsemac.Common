using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchiveFactory :
        IHasSupportedFileFormats {

        IEnumerable<IFileFormat> GetWritableFileFormats();

        IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions);

    }

}
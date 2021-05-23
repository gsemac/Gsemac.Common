using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO {

    public interface IFileFormatFactory {

        IEnumerable<IFileFormat> GetKnownFileFormats();

        IFileFormat FromMimeType(IMimeType mimeType);
        IFileFormat FromFileExtension(string fileExtension);
        IFileFormat FromStream(Stream stream);

    }

}
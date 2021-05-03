﻿using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IFileFormatFactory {

        IEnumerable<IFileFormat> GetKnownFileFormats();

        IFileFormat FromMimeType(IMimeType mimeType);
        IFileFormat FromFileExtension(string fileExtension);

    }

}
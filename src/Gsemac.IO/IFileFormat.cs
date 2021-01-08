using System;
using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IFileFormat :
        IComparable,
        IComparable<IFileFormat> {

        IEnumerable<string> Extensions { get; }
        IEnumerable<IFileSignature> Signatures { get; }
        string MimeType { get; }

    }

}
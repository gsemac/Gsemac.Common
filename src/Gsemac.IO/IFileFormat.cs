using System;
using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IFileFormat :
        IComparable,
        IComparable<IFileFormat>,
        IEquatable<IFileFormat> {

        IEnumerable<string> Extensions { get; }
        IEnumerable<IFileSignature> Signatures { get; }
        IEnumerable<IMimeType> MimeTypes { get; }

    }

}
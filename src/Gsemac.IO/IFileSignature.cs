using System.Collections.Generic;

namespace Gsemac.IO {

    public interface IFileSignature :
        IEnumerable<byte?> {

        byte? this[int index] { get; }

    }

}
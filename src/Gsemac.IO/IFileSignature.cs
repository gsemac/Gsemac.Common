using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO {

    public interface IFileSignature :
        IEnumerable<byte?> {

        byte? this[int index] { get; }

        int Length { get; }

        bool IsMatch(Stream stream);

    }

}
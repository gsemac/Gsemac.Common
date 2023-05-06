using System;

namespace Gsemac.IO {

    public interface ICodecCapabilities :
        IComparable,
        IComparable<ICodecCapabilities> {

        IFileFormat Format { get; }
        bool CanRead { get; }
        bool CanWrite { get; }

    }

}
using System;

namespace Gsemac.IO.Compression {

    public interface IArchiveEntry {

        string Comment { get; }
        long CompressedSize { get; }
        DateTimeOffset LastModified { get; }
        string Name { get; }
        long Size { get; }

    }

}
using System;

namespace Gsemac.IO.Compression {

    public interface IArchiveEntry {

        string Comment { get; }
        long CompressedSize { get; }
        DateTimeOffset LastModified { get; }
        long Size { get; }
        string Path { get; }

    }

}
using System;

namespace Gsemac.IO.Compression.SevenZip {

    internal class BinSevenZipArchiveEntry :
        IArchiveEntry {

        public string Comment { get; set; }
        public long CompressedSize { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }

    }

}
using System;

namespace Gsemac.IO.Compression {

    internal class SharpCompressSevenZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Comment => string.Empty;
        public long CompressedSize => BaseEntry.CompressedSize;
        public DateTimeOffset LastModified => BaseEntry.LastModifiedTime ?? new DateTimeOffset();
        public string Name => BaseEntry.Key;
        public long Size => BaseEntry.Size;

        public SharpCompress.Archives.SevenZip.SevenZipArchiveEntry BaseEntry { get; }

        public SharpCompressSevenZipArchiveEntry(SharpCompress.Archives.SevenZip.SevenZipArchiveEntry entry) {

            BaseEntry = entry;

        }

    }

}
using System;

namespace Gsemac.IO.Compression {

    internal class SharpCompressZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Comment => BaseEntry.Comment;
        public long CompressedSize => BaseEntry.CompressedSize;
        public DateTimeOffset LastModified => BaseEntry.LastModifiedTime ?? new DateTimeOffset();
        public string Name => BaseEntry.Key;
        public long Size => BaseEntry.Size;

        public SharpCompress.Archives.Zip.ZipArchiveEntry BaseEntry { get; }

        public SharpCompressZipArchiveEntry(SharpCompress.Archives.Zip.ZipArchiveEntry entry) {

            BaseEntry = entry;

        }

    }

}
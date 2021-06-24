using System;

namespace Gsemac.IO.Compression.ZipStorer {

    internal class ZipStorerArchiveEntry :
         IArchiveEntry {

        // Public members

        public string Comment => string.Empty;
        public long CompressedSize => BaseEntry.CompressedSize;
        public DateTimeOffset LastModified => BaseEntry.ModifyTime;
        public string Name => BaseEntry.FilenameInZip;
        public long Size => BaseEntry.FileSize;

        public ZipStorer.ZipFileEntry BaseEntry { get; }

        public ZipStorerArchiveEntry(ZipStorer.ZipFileEntry entry) {

            BaseEntry = entry;

        }

    }

}
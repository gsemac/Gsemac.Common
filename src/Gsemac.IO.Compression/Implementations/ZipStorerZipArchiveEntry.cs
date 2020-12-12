using System;
using System.IO.Compression;

namespace Gsemac.IO.Compression.Implementations {

    internal class ZipStorerZipArchiveEntry :
         IArchiveEntry {

        // Public members

        public string Comment => string.Empty;
        public long CompressedSize => BaseEntry.CompressedSize;
        public DateTimeOffset LastModified => BaseEntry.ModifyTime;
        public string Name => BaseEntry.FilenameInZip;
        public long Size => BaseEntry.FileSize;

        public ZipStorer.ZipFileEntry BaseEntry { get; }

        public ZipStorerZipArchiveEntry(ZipStorer.ZipFileEntry entry) {

            this.BaseEntry = entry;

        }

    }

}
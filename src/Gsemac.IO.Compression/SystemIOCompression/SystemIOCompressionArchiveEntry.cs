﻿#if NET45_OR_NEWER

using System;

namespace Gsemac.IO.Compression.SystemIOCompression {

    internal class SystemIOCompressionArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Comment => string.Empty;
        public long CompressedSize => BaseEntry.CompressedLength;
        public DateTimeOffset LastModified => BaseEntry.LastWriteTime;
        public string Name => BaseEntry.FullName;
        public long Size => BaseEntry.Length;

        public System.IO.Compression.ZipArchiveEntry BaseEntry { get; }

        public SystemIOCompressionArchiveEntry(System.IO.Compression.ZipArchiveEntry entry) {

            this.BaseEntry = entry;

        }

    }

}

#endif
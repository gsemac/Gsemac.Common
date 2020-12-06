#if NETFRAMEWORK45_OR_NEWER

namespace Gsemac.IO.Compression.Implementations.Internal {

    internal class SystemIOCompressionZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Comment => string.Empty;
        public long CompressedSize => BaseEntry.CompressedLength;
        public string Path => BaseEntry.FullName;
        public long Size => BaseEntry.Length;

        public System.IO.Compression.ZipArchiveEntry BaseEntry { get; }

        public SystemIOCompressionZipArchiveEntry(System.IO.Compression.ZipArchiveEntry entry) {

            this.BaseEntry = entry;

        }

    }

}

#endif
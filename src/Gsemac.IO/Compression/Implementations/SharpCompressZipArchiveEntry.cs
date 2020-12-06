namespace Gsemac.IO.Compression.Implementations.Internal {

    internal class SharpCompressZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Comment => BaseEntry.Comment;
        public long CompressedSize => BaseEntry.CompressedSize;
        public string Path => BaseEntry.Key;
        public long Size => BaseEntry.Size;

        public SharpCompress.Archives.Zip.ZipArchiveEntry BaseEntry { get; }

        public SharpCompressZipArchiveEntry(SharpCompress.Archives.Zip.ZipArchiveEntry entry) {

            BaseEntry = entry;

        }

    }

}
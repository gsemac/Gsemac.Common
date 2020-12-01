namespace Gsemac.IO.Compression.Implementations.Internal {

    internal class SharpCompressZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Path => BaseEntry.Key;

        public SharpCompress.Archives.Zip.ZipArchiveEntry BaseEntry { get; }

        public SharpCompressZipArchiveEntry(SharpCompress.Archives.Zip.ZipArchiveEntry entry) {

            BaseEntry = entry;

        }

    }

}
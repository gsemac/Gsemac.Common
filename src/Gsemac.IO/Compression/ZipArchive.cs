using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public class ZipArchive :
        IArchive {

        // Public members

        public bool CanRead => underlyingArchive.CanRead;
        public bool CanWrite => underlyingArchive.CanWrite;
        public CompressionLevel CompressionLevel {
            get => underlyingArchive.CompressionLevel;
            set => underlyingArchive.CompressionLevel = value;
        }

        public ZipArchive(string filePath, FileAccess fileAccess = FileAccess.ReadWrite) {

#if NETCORE || NETSTANDARD

            underlyingArchive = new Internal.SystemIOCompressionZipArchive(filePath, fileAccess);

#endif

        }
        public ZipArchive(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite) {

#if NETCORE || NETSTANDARD

            underlyingArchive = new Internal.SystemIOCompressionZipArchive(stream, fileAccess);

#endif

        }

        public void AddEntry(Stream stream, string pathInArchive) => underlyingArchive.AddEntry(stream, pathInArchive);
        public IArchiveEntry GetEntry(string pathInArchive) => underlyingArchive.GetEntry(pathInArchive);
        public void DeleteEntry(IArchiveEntry entry) => underlyingArchive.DeleteEntry(entry);
        public void ExtractEntry(IArchiveEntry entry, Stream outputStream) => underlyingArchive.ExtractEntry(entry, outputStream);
        public IEnumerable<IArchiveEntry> GetEntries() => underlyingArchive.GetEntries();

        public void SaveTo(Stream outputStream) => underlyingArchive.SaveTo(outputStream);

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    underlyingArchive.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly IArchive underlyingArchive;
        private bool disposedValue = false;

    }

}
using Gsemac.IO.Compression.Implementations;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public class ZipArchive :
        IArchive {

        // Public members

        public bool CanRead => underlyingArchive.CanRead;
        public bool CanWrite => underlyingArchive.CanWrite;
        public string Comment {
            get => underlyingArchive.Comment;
            set => underlyingArchive.Comment = value;
        }
        public CompressionLevel CompressionLevel {
            get => underlyingArchive.CompressionLevel;
            set => underlyingArchive.CompressionLevel = value;
        }

        public ZipArchive() {

#if NETFRAMEWORK45_OR_NEWER

            underlyingArchive = new SystemIOCompressionZipArchive();
#else
            underlyingArchive = new SharpCompressZipArchive();
#endif

        }
        public ZipArchive(string filePath, FileAccess fileAccess = FileAccess.ReadWrite) {

#if NETFRAMEWORK45_OR_NEWER
            underlyingArchive = new SystemIOCompressionZipArchive(filePath, fileAccess);
#else
            underlyingArchive = new SharpCompressZipArchive(filePath, fileAccess);
#endif

        }
        public ZipArchive(Stream stream) {

#if NETFRAMEWORK45_OR_NEWER
            underlyingArchive = new SystemIOCompressionZipArchive(stream);
#else
            underlyingArchive = new SharpCompressZipArchive(stream);
#endif

        }

        public IArchiveEntry AddEntry(Stream stream, string entryName, bool leaveOpen = false) => underlyingArchive.AddEntry(stream, entryName, leaveOpen);
        public IArchiveEntry GetEntry(string entryName) => underlyingArchive.GetEntry(entryName);
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
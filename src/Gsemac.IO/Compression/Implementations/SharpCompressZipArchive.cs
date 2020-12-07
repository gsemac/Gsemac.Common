using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.Implementations {

    public class SharpCompressZipArchive :
        IArchive {

        // Public members

        public bool CanRead => fileAccess.HasFlag(FileAccess.Read);
        public bool CanWrite => fileAccess.HasFlag(FileAccess.Write);
        public string Comment {
            get => throw ArchiveExceptions.ReadingCommentsIsNotSupported;
            set => throw ArchiveExceptions.WritingCommentsIsNotSupported;
        } // SharpCompress offers no means to set the archive comment outside of ZipWriterOptions, which can't be passed to SaveTo
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public SharpCompressZipArchive(string filePath, FileAccess fileAccess = FileAccess.ReadWrite) :
            this(new FileStream(filePath, FileAccess.ReadWrite.HasFlag(FileAccess.Write) ? FileMode.OpenOrCreate : FileMode.Open, fileAccess), leaveOpen: false) {

            this.fileAccess = fileAccess;

        }
        public SharpCompressZipArchive(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite) :
            this(stream, leaveOpen: true) {

            this.fileAccess = fileAccess;

        }

        public IArchiveEntry AddEntry(Stream stream, string entryName, bool leaveOpen = false) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(entryName))
                throw new ArgumentNullException(nameof(entryName));

            // The ZipArchive class does not overwrite existing files if one already exists at the same path, but adds a duplicate.
            // To simulate overwriting the existing file, we'll delete the original file first if it is present.

            IArchiveEntry existingEntry = GetEntry(entryName);

            if (!(existingEntry is null))
                DeleteEntry(existingEntry);

            archiveModified = true;

            return new SharpCompressZipArchiveEntry(archive.AddEntry(entryName, stream, closeStream: !leaveOpen));

        }
        public IArchiveEntry GetEntry(string entryName) {

            SharpCompress.Archives.Zip.ZipArchiveEntry entry = archive.Entries.Where(e => e.Key.Equals(entryName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return entry is null ? null : new SharpCompressZipArchiveEntry(entry);

        }
        public void DeleteEntry(IArchiveEntry entry) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry is SharpCompressZipArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive)
                archive.RemoveEntry(zipArchiveEntry.BaseEntry);
            else
                throw ArchiveExceptions.EntryDoesNotBelongToThisArchive;

            archiveModified = true;

        }
        public void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is SharpCompressZipArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive) {

                using (Stream entryStream = zipArchiveEntry.BaseEntry.OpenEntryStream())
                    entryStream.CopyTo(outputStream);

            }
            else
                throw ArchiveExceptions.EntryDoesNotBelongToThisArchive;

        }
        public IEnumerable<IArchiveEntry> GetEntries() {

            return archive.Entries.Where(entry => !entry.IsDirectory)
                .Select(entry => new SharpCompressZipArchiveEntry(entry));

        }

        public void SaveTo(Stream outputStream) {

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            archive.DeflateCompressionLevel = GetDeflateCompressionLevel(CompressionLevel);

            archive.SaveTo(outputStream, new SharpCompress.Writers.WriterOptions(GetCompressionType(CompressionLevel)) {
                LeaveStreamOpen = true,
            });

        }
        public void Close() {

            if (!archiveIsClosed)
                CommitChanges();

            archiveIsClosed = true;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Close();

                    archive.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly FileAccess fileAccess = FileAccess.ReadWrite;
        private readonly bool canWriteDirectoryToSourceStream = false;
        private readonly SharpCompress.Archives.Zip.ZipArchive archive;
        private Stream sourceStream = null;
        private bool archiveModified = false;
        private bool disposedValue = false;
        private bool archiveIsClosed = false;

        private SharpCompressZipArchive(Stream stream, bool leaveOpen) {

            sourceStream = stream;

            // It will complain if we try to open a ZIP archive that doesn't exist (can't read headers).
            // Create an empty ZIP archive for it to open.

            if (stream.Length <= 0) {

                using (SharpCompress.Archives.Zip.ZipArchive tempArchive = SharpCompress.Archives.Zip.ZipArchive.Create())
                    tempArchive.SaveTo(stream);

                stream.Seek(0, SeekOrigin.Begin);

                // We're creating a new archive, we can write directly to the stream without worrying about affecting existing entries.

                canWriteDirectoryToSourceStream = true;

            }

            archive = SharpCompress.Archives.Zip.ZipArchive.Open(stream, new SharpCompress.Readers.ReaderOptions() {
                LeaveStreamOpen = leaveOpen
            });

        }

        private SharpCompress.Common.CompressionType GetCompressionType(CompressionLevel compressionLevel) {

            switch (compressionLevel) {

                case CompressionLevel.Store:
                    return SharpCompress.Common.CompressionType.None;

                case CompressionLevel.Fastest:
                case CompressionLevel.Maximum:
                    return SharpCompress.Common.CompressionType.Deflate;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionLevel));

            }

        }
        private SharpCompress.Compressors.Deflate.CompressionLevel GetDeflateCompressionLevel(CompressionLevel compressionLevel) {

            switch (compressionLevel) {

                case CompressionLevel.Store:
                    return SharpCompress.Compressors.Deflate.CompressionLevel.None;

                case CompressionLevel.Fastest:
                    return SharpCompress.Compressors.Deflate.CompressionLevel.BestSpeed;

                case CompressionLevel.Maximum:
                    return SharpCompress.Compressors.Deflate.CompressionLevel.BestCompression;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionLevel));

            }

        }

        private void CommitChanges() {

            if (!fileAccess.HasFlag(FileAccess.Write) || sourceStream is null || !archiveModified)
                return;

            if (canWriteDirectoryToSourceStream) {

                sourceStream.Seek(0, SeekOrigin.Begin);

                SaveTo(sourceStream);

            }
            else {

                // Since entries are read from the stream lazily, we can't write directly to the source stream.
                // Save a temporary copy of the archive, and replace the old one.

                string tempArchiveFilePath = Path.GetTempFileName();

                using (FileStream fs = new FileStream(tempArchiveFilePath, FileMode.Create)) {

                    SaveTo(fs);

                    fs.Seek(0, SeekOrigin.Begin);

                    sourceStream.Seek(0, SeekOrigin.Begin);
                    sourceStream.SetLength(fs.Length);

                    fs.CopyTo(sourceStream);

                }

                File.Delete(tempArchiveFilePath);

            }

            // Make sure we do not attempt to do this more than once.

            sourceStream = null;

        }

    }

}
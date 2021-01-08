using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression {

    internal class SharpCompress7ZipArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => true;
        public override bool CanWrite => false; // SharpCompress does not support writing to .7Z archives
        public override string Comment {
            get => throw new NotSupportedException("Archive does not support reading archive-level comments.");
            set => throw new NotSupportedException("Archive does not support writing archive-level comments.");
        }
        public override CompressionLevel CompressionLevel {
            get => throw new NotSupportedException("Archive does not support reading the compression level.");
            set => throw new NotSupportedException("Archive does not support setting the compression level.");
        }

        public SharpCompress7ZipArchive(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            if (fileAccess.HasFlag(FileAccess.Write))
                throw new NotSupportedException("The archive can only be opened in read-only mode.");

            if (options is null)
                options = ArchiveOptions.Default;

            archive = SharpCompress.Archives.SevenZip.SevenZipArchive.Open(stream, new SharpCompress.Readers.ReaderOptions() {
                LeaveStreamOpen = leaveOpen,
                ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding() {
                    Default = options.Encoding ?? Encoding.UTF8,
                }
            });

        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, bool overwrite = true, bool leaveOpen = false, IArchiveEntryOptions options = null) {

            throw new UnauthorizedAccessException("The archive is read-only.");

        }
        public override IArchiveEntry GetEntry(string entryName) {

            entryName = SanitizeEntryName(entryName);

            IArchiveEntry entry = GetEntries().Where(e => PathUtilities.AreEqual(e.Name, entryName))
                .FirstOrDefault();

            return entry;

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            throw new UnauthorizedAccessException("The archive is read-only.");

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is SharpCompress7ZipArchiveEntry sevenZipArchiveEntry && sevenZipArchiveEntry.BaseEntry.Archive == archive) {

                using (Stream entryStream = sevenZipArchiveEntry.BaseEntry.OpenEntryStream())
                    entryStream.CopyTo(outputStream);

            }
            else
                throw new ArchiveEntryDoesNotExistException();

        }
        public override IEnumerable<IArchiveEntry> GetEntries() {

            return archive.Entries.Where(entry => !entry.IsDirectory)
                .Select(entry => new SharpCompress7ZipArchiveEntry(entry));

        }

        public override void Close() {

            Dispose();

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing)
                    archive.Dispose();

                disposedValue = true;

            }

        }

        // Private members

        private bool disposedValue = true;
        private readonly SharpCompress.Archives.SevenZip.SevenZipArchive archive;
    }

}
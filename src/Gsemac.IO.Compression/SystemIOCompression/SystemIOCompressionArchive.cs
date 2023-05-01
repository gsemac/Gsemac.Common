#if NET45_OR_NEWER

using Gsemac.IO.Compression.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression.SystemIOCompression {

    internal class SystemIOCompressionArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => archive.Mode == System.IO.Compression.ZipArchiveMode.Read || archive.Mode == System.IO.Compression.ZipArchiveMode.Update;
        public override bool CanWrite => archive.Mode == System.IO.Compression.ZipArchiveMode.Create || archive.Mode == System.IO.Compression.ZipArchiveMode.Update;
        public override string Comment {
            get => throw new NotSupportedException(ExceptionMessages.ArchiveDoesNotSupportReadingComments);
            set => throw new NotSupportedException(ExceptionMessages.ArchiveDoesNotSupportWritingComments);
        }
        public override CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public SystemIOCompressionArchive(Stream stream, IArchiveOptions options = null) {

            if (options is null)
                options = new ArchiveOptions();

            this.CompressionLevel = options.CompressionLevel;

            //this.stream = stream;
            this.archive = new System.IO.Compression.ZipArchive(stream, GetZipArchiveMode(options.FileAccess), options.LeaveStreamOpen, options.Encoding ?? Encoding.UTF8);

        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(entryName))
                throw new ArgumentNullException(nameof(entryName));

            if (options is null)
                options = ArchiveEntryOptions.Default;

            // The ZipArchive class does not overwrite existing files if one already exists at the same path, but adds a duplicate.
            // To simulate overwriting the existing file, we'll delete the original file first if it is present.

            IArchiveEntry existingEntry = GetEntry(entryName);

            if (!(existingEntry is null)) {

                if (options.Overwrite)
                    DeleteEntry(existingEntry);
                else
                    throw new ArchiveEntryExistsException();

            }

            System.IO.Compression.ZipArchiveEntry entry = archive.CreateEntry(SanitizeEntryName(entryName), GetCompressionLevel(CompressionLevel));

            using (Stream entryStream = entry.Open())
                stream.CopyTo(entryStream);

            // We can close the stream immediately since it's already copied to the archive.

            if (!options.LeaveStreamOpen) {

                stream.Close();
                stream.Dispose();

            }

            return new SystemIOCompressionArchiveEntry(entry);

        }
        public override IArchiveEntry GetEntry(string entryName) {

            System.IO.Compression.ZipArchiveEntry entry = archive.GetEntry(SanitizeEntryName(entryName));

            return entry is null ? null : new SystemIOCompressionArchiveEntry(entry);

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry is SystemIOCompressionArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive)
                zipArchiveEntry.BaseEntry.Delete();
            else
                throw new ArchiveEntryDoesNotExistException();

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is SystemIOCompressionArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive) {

                using (Stream entryStream = zipArchiveEntry.BaseEntry.Open())
                    entryStream.CopyTo(outputStream);

            }
            else
                throw new ArchiveEntryDoesNotExistException();

        }

        public override IEnumerable<IArchiveEntry> GetEntries() {

            return archive.Entries
                .Where(entry => PathUtilities.IsFilePath(entry.FullName))
                .Select(entry => new SystemIOCompressionArchiveEntry(entry));

        }

        public override void Close() { }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Close();

                    archive.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        //private readonly Stream stream;
        private readonly System.IO.Compression.ZipArchive archive;
        private bool disposedValue = false;

        private System.IO.Compression.ZipArchiveMode GetZipArchiveMode(FileAccess fileAccess) {

            switch (fileAccess) {

                case FileAccess.Read:
                    return System.IO.Compression.ZipArchiveMode.Read;

                case FileAccess.Write:
                    return System.IO.Compression.ZipArchiveMode.Create;

                case FileAccess.ReadWrite:
                    return System.IO.Compression.ZipArchiveMode.Update;

                default:
                    throw new ArgumentOutOfRangeException(nameof(fileAccess));

            }

        }
        private System.IO.Compression.CompressionLevel GetCompressionLevel(CompressionLevel compressionLevel) {

            switch (compressionLevel) {

                case CompressionLevel.Store:
                    return System.IO.Compression.CompressionLevel.NoCompression;

                case CompressionLevel.Maximum:
                    return System.IO.Compression.CompressionLevel.Optimal;

                case CompressionLevel.Fastest:
                    return System.IO.Compression.CompressionLevel.Fastest;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionLevel));

            }

        }

        //private void SaveTo(Stream outputStream) {

        //    if (outputStream is null)
        //        throw new ArgumentNullException(nameof(outputStream));

        //    long position = stream.Position;

        //    stream.Seek(0, SeekOrigin.Begin);

        //    stream.CopyTo(outputStream);

        //    stream.Seek(position, SeekOrigin.Begin);

        //}

    }

}

#endif
﻿#if NET45_OR_NEWER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression.Implementations {

    public class SystemIOCompressionZipArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => archive.Mode == System.IO.Compression.ZipArchiveMode.Read || archive.Mode == System.IO.Compression.ZipArchiveMode.Update;
        public override bool CanWrite => archive.Mode == System.IO.Compression.ZipArchiveMode.Create || archive.Mode == System.IO.Compression.ZipArchiveMode.Update;
        public override string Comment {
            get => throw ArchiveExceptions.ReadingCommentsIsNotSupported;
            set => throw ArchiveExceptions.WritingCommentsIsNotSupported;
        }
        public override CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public SystemIOCompressionZipArchive(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) :
            this(stream, true, fileAccess, options) {
        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, bool overwrite = true, bool leaveOpen = false, IArchiveEntryOptions options = null) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(entryName))
                throw new ArgumentNullException(nameof(entryName));

            // The ZipArchive class does not overwrite existing files if one already exists at the same path, but adds a duplicate.
            // To simulate overwriting the existing file, we'll delete the original file first if it is present.

            IArchiveEntry existingEntry = GetEntry(entryName);

            if (!(existingEntry is null)) {

                if (overwrite)
                    DeleteEntry(existingEntry);
                else
                    throw ArchiveExceptions.EntryAlreadyExists;

            }

            System.IO.Compression.ZipArchiveEntry entry = archive.CreateEntry(entryName, GetCompressionLevel(CompressionLevel));

            using (Stream entryStream = entry.Open())
                stream.CopyTo(entryStream);

            // We can close the stream immediately since it's already copied to the archive.

            if (!leaveOpen)
                stream.Dispose();

            return new SystemIOCompressionZipArchiveEntry(entry);

        }
        public override IArchiveEntry GetEntry(string entryName) {

            System.IO.Compression.ZipArchiveEntry entry = archive.GetEntry(entryName);

            return entry is null ? null : new SystemIOCompressionZipArchiveEntry(entry);

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry is SystemIOCompressionZipArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive)
                zipArchiveEntry.BaseEntry.Delete();
            else
                throw ArchiveExceptions.EntryDoesNotBelongToThisArchive;

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is SystemIOCompressionZipArchiveEntry zipArchiveEntry && zipArchiveEntry.BaseEntry.Archive == archive) {

                using (Stream entryStream = zipArchiveEntry.BaseEntry.Open())
                    entryStream.CopyTo(outputStream);

            }
            else
                throw ArchiveExceptions.EntryDoesNotBelongToThisArchive;

        }

        public override IEnumerable<IArchiveEntry> GetEntries() {

            return archive.Entries
                .Where(entry => PathUtilities.IsFilePath(entry.FullName))
                .Select(entry => new SystemIOCompressionZipArchiveEntry(entry));

        }

        public override void Close() { }

        public override void Dispose() {

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

        //private readonly Stream stream;
        private readonly System.IO.Compression.ZipArchive archive;
        private bool disposedValue = false;

        private SystemIOCompressionZipArchive(Stream stream, bool leaveOpen, FileAccess fileAccess, IArchiveOptions options) {

            if (options is null)
                options = new ArchiveOptions();

            this.CompressionLevel = options.CompressionLevel;

            //this.stream = stream;
            this.archive = new System.IO.Compression.ZipArchive(stream, GetZipArchiveMode(fileAccess), leaveOpen, options.Encoding ?? Encoding.UTF8);

        }

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
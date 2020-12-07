﻿using Gsemac.IO.Compression.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression.Implementations {

    public class ZipStorerZipArchive :
      ArchiveBase {

        // Public members

        public override bool CanRead => fileAccess.HasFlag(FileAccess.Read);
        public override bool CanWrite => fileAccess.HasFlag(FileAccess.Write);
        public override string Comment {
            get => throw ArchiveExceptions.ReadingCommentsIsNotSupported;
            set {

                if (archive?.IsValueCreated ?? false)
                    throw ArchiveExceptions.WritingCommentsIsNotSupported;

                archiveComment = value;

            }
        }
        public override CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public ZipStorerZipArchive(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, IArchiveOptions options = null) :
            this(stream, leaveOpen: true, fileAccess, options) {
        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, bool leaveOpen = false) {

            if (!CanWrite)
                throw ArchiveExceptions.ArchiveIsReadOnly;

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(entryName))
                throw new ArgumentNullException(nameof(entryName));

            // The ZipArchive class does not overwrite existing files if one already exists at the same path, but adds a duplicate.
            // To simulate overwriting the existing file, we'll delete the original file first if it is present.

            IArchiveEntry existingEntry = GetEntry(entryName);

            if (!(existingEntry is null))
                DeleteEntry(existingEntry);

            ZipStorer.ZipFileEntry entry = archive.Value.AddStream(GetCompression(CompressionLevel), entryName, stream, DateTime.Now);

            entries.Add(new ZipStorerZipArchiveEntry(entry));

            // We can close the stream immediately since it's already copied to the archive.

            if (!leaveOpen)
                stream.Dispose();

            return entries.Last();

        }
        public override IArchiveEntry GetEntry(string entryName) {

            return GetEntries().Where(entry => entry.Path.Equals(entryName))
                .FirstOrDefault();

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (!CanWrite)
                throw ArchiveExceptions.ArchiveIsReadOnly;

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry is ZipStorerZipArchiveEntry zipArchiveEntry && entries.Contains(zipArchiveEntry)) {

                entries.Remove(zipArchiveEntry);
                deletedEntries.Add(zipArchiveEntry);

            }
            else
                throw ArchiveExceptions.EntryDoesNotBelongToThisArchive;

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is ZipStorerZipArchiveEntry zipArchiveEntry)
                archive.Value.ExtractFile(zipArchiveEntry.BaseEntry, outputStream);
            else
                throw new ArgumentException("Entry does not belong to this archive.", nameof(entry));

        }

        public override IEnumerable<IArchiveEntry> GetEntries() {

            return entries;

        }

        public override void Close() {

            if (!archiveIsClosed)
                CommitChanges();

        }

        public override void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Close();

                    // The Close method will close the underlying archive.
                    // Dispose only calls Close, so to avoid calling Close twice, we'll only call it if it's not already closed.

                    if (archive.IsValueCreated && !archiveIsClosed)
                        archive.Value.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly FileAccess fileAccess = FileAccess.ReadWrite;
        private readonly Stream sourceStream;
        private readonly Lazy<ZipStorer> archive;
        private readonly List<ZipStorerZipArchiveEntry> entries = new List<ZipStorerZipArchiveEntry>();
        private readonly List<ZipStorerZipArchiveEntry> deletedEntries = new List<ZipStorerZipArchiveEntry>();
        private string archiveComment;
        private bool disposedValue = false;
        private bool archiveIsClosed = false;

        private ZipStorerZipArchive(Stream stream, bool leaveOpen, FileAccess fileAccess, IArchiveOptions options) {

            if (options is null)
                options = new ArchiveOptions();

            this.Comment = options.Comment;
            this.CompressionLevel = options.CompressionLevel;

            this.fileAccess = fileAccess;
            this.sourceStream = stream;

            if (stream.Length > 0) {

                // ZipStorer has a longstanding bug where calling ReadCentralDir and then adding to the archive causes the last entry to get corrupted.
                // To get around this, we'll read the entries, and then reload the archive. Read-only access is IMPORTANT to keep it from corrupting the archive.

                long streamPosition = stream.Position;

                using (ZipStorer archive = ZipStorer.Open(stream, FileAccess.Read, leaveOpen)) {

                    try {

                        entries = new List<ZipStorerZipArchiveEntry>(archive.ReadCentralDir().Where(entry => PathUtilities.IsFilePath(entry.FilenameInZip))
                            .Select(entry => new ZipStorerZipArchiveEntry(entry)));

                    }
                    catch (InvalidOperationException) {

                        // ReadCentralDir can throw for new archives (i.e. ones we didn't open).

                        entries = new List<ZipStorerZipArchiveEntry>();

                    }

                }

                stream.Position = streamPosition;

            }

            // Archive is created lazily to give the user a chance to edit the archive comment.

            this.archive = new Lazy<ZipStorer>(() => {

                ZipStorer archive;

                if (stream.Length <= 0)
                    archive = ZipStorer.Create(stream, archiveComment, leaveOpen);
                else
                    archive = ZipStorer.Open(stream, fileAccess, leaveOpen);

                if (options.Encoding == Encoding.UTF8)
                    archive.EncodeUTF8 = true;

                return archive;

            });

        }

        private ZipStorer.Compression GetCompression(CompressionLevel compressionLevel) {

            switch (compressionLevel) {

                case CompressionLevel.Store:
                    return ZipStorer.Compression.Store;

                case CompressionLevel.Maximum:
                    return ZipStorer.Compression.Deflate;

                case CompressionLevel.Fastest:
                    return ZipStorer.Compression.Deflate;

                default:
                    throw new ArgumentOutOfRangeException(nameof(compressionLevel));

            }

        }

        private void CommitChanges() {

            if (deletedEntries.Any()) {

                // If any files were deleted, we need to recreate the ZIP archive with the files omitted.

                // Close the archive to flush it to the output stream.

                if (archive.IsValueCreated)
                    archive.Value.Close();

                archiveIsClosed = true;

                string tempDirectory = PathUtilities.GetUniqueTemporaryDirectoryPath();

                try {

                    // Extract the archive to a temporary directory, excluding deleted files.

                    sourceStream.Seek(0, SeekOrigin.Begin);

                    using (ZipStorerZipArchive tempArchive = new ZipStorerZipArchive(sourceStream, FileAccess.Read))
                        foreach (IArchiveEntry entry in tempArchive.GetEntries().Where(e => !deletedEntries.Any(deletedEntry => deletedEntry.LastModified == e.LastModified && deletedEntry.Path.Equals(e.Path))))
                            tempArchive.ExtractEntry(entry, Path.Combine(tempDirectory, entry.Path));

                    // Recreate the archive with only the non-deleted files.

                    sourceStream.Seek(0, SeekOrigin.Begin);
                    sourceStream.SetLength(0);

                    using (ZipStorerZipArchive tempArchive = new ZipStorerZipArchive(sourceStream, FileAccess.Write))
                        tempArchive.AddAllFiles(tempDirectory);

                }
                finally {

                    Directory.Delete(tempDirectory, true);

                }

            }

        }

    }

}
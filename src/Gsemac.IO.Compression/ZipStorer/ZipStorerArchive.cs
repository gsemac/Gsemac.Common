﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression.ZipStorer {

    internal class ZipStorerArchive :
      ArchiveBase {

        // Public members

        public override bool CanRead => fileAccess.HasFlag(FileAccess.Read);
        public override bool CanWrite => fileAccess.HasFlag(FileAccess.Write);
        public override string Comment {
            get => throw new NotSupportedException("Archive does not support reading archive-level comments.");
            set {

                if (archive?.IsValueCreated ?? false)
                    throw new InvalidOperationException("Archive does not support writing archive-level comments.");

                archiveComment = value;

            }
        }
        public override CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;

        public ZipStorerArchive(Stream stream, IArchiveOptions options) {

            if (options is null)
                options = new ArchiveOptions();

            Comment = options.Comment;
            CompressionLevel = options.CompressionLevel;

            fileAccess = options.FileAccess;
            sourceStream = stream;
            leaveStreamOpen = options.LeaveStreamOpen;

            if (stream.Length > 0) {

                // ZipStorer has a longstanding bug where calling ReadCentralDir and then adding to the archive causes the last entry to get corrupted.
                // To get around this, we'll read the entries, and then reload the archive. Read-only access is IMPORTANT to keep it from corrupting the archive.

                long streamPosition = stream.Position;

                using (ZipStorer archive = ZipStorer.Open(stream, FileAccess.Read, _leaveOpen: true)) {

                    try {

                        entries = new List<ZipStorerArchiveEntry>(archive.ReadCentralDir().Where(entry => PathUtilities.IsFilePath(entry.FilenameInZip))
                            .Select(entry => new ZipStorerArchiveEntry(entry)));

                    }
                    catch (InvalidOperationException) {

                        // ReadCentralDir can throw for new archives (i.e. ones we didn't open).

                        entries = new List<ZipStorerArchiveEntry>();

                    }

                }

                stream.Position = streamPosition;

            }

            // Archive is created lazily to give the user a chance to edit the archive comment.

            archive = new Lazy<ZipStorer>(() => {

                ZipStorer archive;

                if (stream.Length <= 0)
                    archive = ZipStorer.Create(stream, archiveComment, _leaveOpen: true);
                else
                    archive = ZipStorer.Open(stream, fileAccess, _leaveOpen: true);

                if (options.Encoding == Encoding.UTF8)
                    archive.EncodeUTF8 = true;

                return archive;

            });

        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(entryName))
                throw new ArgumentNullException(nameof(entryName));

            if (options is null)
                options = ArchiveEntryOptions.Default;

            // The ZipArchive class does not overwrite existing files if one already exists at the same path, but adds a duplicate.
            // To simulate overwriting the existing file, we'll delete the original file first if it is present.

            IArchiveEntry existingEntry = GetEntry(entryName);

            if (existingEntry is object) {

                if (options.Overwrite)
                    DeleteEntry(existingEntry);
                else
                    throw new ArchiveEntryExistsException();

            }

            ZipStorer.ZipFileEntry entry = archive.Value.AddStream(GetCompression(CompressionLevel), SanitizeEntryName(entryName), stream, DateTime.Now);

            entries.Add(new ZipStorerArchiveEntry(entry));

            // We can close the stream immediately since it's already copied to the archive.

            if (!options.LeaveStreamOpen) {

                stream.Close();
                stream.Dispose();

            }

            return entries.Last();

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry is ZipStorerArchiveEntry zipArchiveEntry && entries.Contains(zipArchiveEntry)) {

                entries.Remove(zipArchiveEntry);
                deletedEntries.Add(zipArchiveEntry);

            }
            else
                throw new ArchiveEntryDoesNotExistException();

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is ZipStorerArchiveEntry zipArchiveEntry)
                archive.Value.ExtractFile(zipArchiveEntry.BaseEntry, outputStream);
            else
                throw new ArgumentException("Entry does not belong to the archive.", nameof(entry));

        }

        public override IEnumerable<IArchiveEntry> GetEntries() {

            return entries;

        }

        public override void Close() {

            if (!archiveIsClosed)
                CommitChanges();

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Close();

                    // The Close method will close the underlying archive.
                    // Dispose only calls Close, so to avoid calling Close twice, we'll only call it if it's not already closed.

                    if (archive.IsValueCreated && !archiveIsClosed)
                        archive.Value.Dispose();

                    if (!leaveStreamOpen) {

                        sourceStream.Close();
                        sourceStream.Dispose();

                    }

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly FileAccess fileAccess = FileAccess.ReadWrite;
        private readonly Stream sourceStream;
        private readonly bool leaveStreamOpen = false;
        private readonly Lazy<ZipStorer> archive;
        private readonly List<ZipStorerArchiveEntry> entries = new List<ZipStorerArchiveEntry>();
        private readonly List<ZipStorerArchiveEntry> deletedEntries = new List<ZipStorerArchiveEntry>();
        private string archiveComment;
        private bool disposedValue = false;
        private bool archiveIsClosed = false;

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
        private bool AreEqual(IArchiveEntry entry1, IArchiveEntry entry2) {

            return entry1.LastModified == entry2.LastModified && entry1.Name.Equals(entry2.Name);

        }

        private void CommitChanges() {

            if (deletedEntries.Any()) {

                // If any files were deleted, we need to recreate the ZIP archive with the files omitted.

                // Close the archive to flush it to the output stream.

                if (archive.IsValueCreated)
                    archive.Value.Close();

                archiveIsClosed = true;

                string tempDirectory = PathUtilities.GetTemporaryDirectoryPath(new TemporaryPathOptions() { EnsureUnique = true, });

                try {

                    // Extract the archive to a temporary directory, excluding deleted files.

                    sourceStream.Seek(0, SeekOrigin.Begin);

                    using (ZipStorerArchive tempArchive = new ZipStorerArchive(sourceStream, new ArchiveOptions { FileAccess = FileAccess.Read, LeaveStreamOpen = true }))
                        foreach (IArchiveEntry entry in tempArchive.GetEntries().Where(e => !deletedEntries.Any(deletedEntry => AreEqual(deletedEntry, e))))
                            tempArchive.ExtractEntry(entry, Path.Combine(tempDirectory, entry.Name));

                    // Recreate the archive with only the non-deleted files.
                    // We need read-write access in order to add files to the archive (for when it checks if the entry already exists).

                    sourceStream.SetLength(0);

                    using (ZipStorerArchive tempArchive = new ZipStorerArchive(sourceStream, new ArchiveOptions { FileAccess = FileAccess.ReadWrite, LeaveStreamOpen = true }))
                        tempArchive.AddAllFiles(tempDirectory);

                }
                finally {

                    Directory.Delete(tempDirectory, true);

                }

            }

        }

    }

}
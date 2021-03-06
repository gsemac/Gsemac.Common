﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO.Compression {

    internal class SharpCompressSevenZipArchive :
        ArchiveBase {

        // Public members

        public override bool CanRead => true;
        public override bool CanWrite => false; // SharpCompress does not support writing to .7Z archives
        public override string Comment {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportReadingComments);
            set => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportWritingComments);
        }
        public override CompressionLevel CompressionLevel {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportGettingCompressionLevel);
            set => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportSettingCompressionLevel);
        }

        public SharpCompressSevenZipArchive(Stream stream, IArchiveOptions options = null) {

            if (options is null)
                options = ArchiveOptions.Default;

            if (options.FileAccess.HasFlag(FileAccess.Write))
                throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportWriting);

            archive = SharpCompress.Archives.SevenZip.SevenZipArchive.Open(stream, new SharpCompress.Readers.ReaderOptions() {
                LeaveStreamOpen = options.LeaveStreamOpen,
                ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding() {
                    Default = options.Encoding ?? Encoding.UTF8,
                }
            });

        }

        public override IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options) {

            throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

        }
        public override IArchiveEntry GetEntry(string entryName) {

            entryName = SanitizeEntryName(entryName);

            IArchiveEntry entry = GetEntries().Where(e => PathUtilities.AreEqual(e.Name, entryName))
                .FirstOrDefault();

            return entry;

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

        }
        public override void ExtractEntry(IArchiveEntry entry, Stream outputStream) {

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (outputStream is null)
                throw new ArgumentNullException(nameof(outputStream));

            if (entry is SharpCompressSevenZipArchiveEntry sevenZipArchiveEntry && sevenZipArchiveEntry.BaseEntry.Archive == archive) {

                using (Stream entryStream = sevenZipArchiveEntry.BaseEntry.OpenEntryStream())
                    entryStream.CopyTo(outputStream);

            }
            else
                throw new ArchiveEntryDoesNotExistException();

        }
        public override IEnumerable<IArchiveEntry> GetEntries() {

            return archive.Entries.Where(entry => !entry.IsDirectory)
                .Select(entry => new SharpCompressSevenZipArchiveEntry(entry));

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
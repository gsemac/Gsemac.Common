using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public abstract class ArchiveBase :
        IArchive {

        // Public members

        public virtual bool CanRead => archiveOptions.FileAccess.HasFlag(FileAccess.Read);
        public virtual bool CanWrite => archiveOptions.FileAccess.HasFlag(FileAccess.Write);
        public virtual string Comment {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportReadingComments);
            set => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportWritingComments);
        }
        public virtual CompressionLevel CompressionLevel {
            get => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportGettingCompressionLevel);
            set => throw new NotSupportedException(Properties.ExceptionMessages.ArchiveDoesNotSupportSettingCompressionLevel);
        }

        public abstract IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options = null);
        public virtual IArchiveEntry GetEntry(string entryName) {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            entryName = SanitizeEntryName(entryName);

            return GetEntries().Where(entry => PathUtilities.AreEqual(SanitizeEntryName(entry.Name), entryName))
                .FirstOrDefault();

        }
        public abstract void DeleteEntry(IArchiveEntry entry);
        public abstract void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        public abstract IEnumerable<IArchiveEntry> GetEntries();

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        public abstract void Close();

        public IEnumerator<IArchiveEntry> GetEnumerator() {

            return GetEntries().GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Protected members

        protected ArchiveBase() :
            this(ArchiveOptions.Default) {
        }
        protected ArchiveBase(IArchiveOptions archiveOptions) {

            this.archiveOptions = archiveOptions;

        }

        protected virtual void Dispose(bool disposing) {

            Close();

        }

        protected static string SanitizeEntryName(string entryName) {

            // Entry names should always be relative paths, and should always be file paths (i.e. not directory paths).

            entryName = PathUtilities.TrimDirectorySeparators(entryName);

            // The ZIP specification states the forward-slash character should always be used as a path separator (for compatibility reasons).
            // https://stackoverflow.com/a/44387973

            entryName = PathUtilities.NormalizeDirectorySeparators(entryName, '/');

            return entryName;

        }

        // Private members

        private readonly IArchiveOptions archiveOptions;

    }

}
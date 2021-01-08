using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public abstract class ArchiveBase :
        IArchive {

        // Public members

        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract string Comment { get; set; }
        public abstract CompressionLevel CompressionLevel { get; set; }

        public abstract IArchiveEntry AddEntry(Stream stream, string entryName, bool overwrite = true, bool leaveOpen = false, IArchiveEntryOptions options = null);
        public abstract IArchiveEntry GetEntry(string entryName);
        public abstract void DeleteEntry(IArchiveEntry entry);
        public abstract void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        public abstract IEnumerable<IArchiveEntry> GetEntries();

        public void Dispose() {

            Dispose(disposing: true);

            System.GC.SuppressFinalize(this);

        }

        public abstract void Close();

        public IEnumerator<IArchiveEntry> GetEnumerator() {

            return GetEntries().GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Protected members

        protected virtual void Dispose(bool disposing) { }

        protected static string SanitizeEntryName(string entryName) {

            // Entry names should always be relative paths, and should always be file paths (i.e. not directory paths).

            entryName = PathUtilities.TrimDirectorySeparators(entryName);

            // The ZIP specification states the forward-slash character should always be used as a path separator (for compatibility reasons).
            // https://stackoverflow.com/a/44387973

            entryName = PathUtilities.NormalizeDirectorySeparators(entryName, '/');

            return entryName;

        }

    }

}
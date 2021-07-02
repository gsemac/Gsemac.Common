using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    internal abstract class DeferredCreationArchiveBase :
        ArchiveBase {

        // Public members

        public override IArchiveEntry AddEntry(Stream stream, string entryName, IArchiveEntryOptions options) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (stream is FileStream fileStream) {

                if (options is null)
                    options = ArchiveEntryOptions.Default;

                IArchiveEntry existingEntry = GetEntry(entryName);

                NewArchiveEntry newEntry = new NewArchiveEntry() {
                    Name = SanitizeEntryName(entryName),
                    LastModified = DateTimeOffset.Now,
                    Comment = options.Comment,
                    FilePath = Path.GetFullPath(fileStream.Name),
                };

                if (existingEntry is object) {

                    if (!options.Overwrite) {

                        throw new ArchiveEntryExistsException();

                    }
                    else if (newEntry.RenameRequired) {

                        // While 7-Zip/WinRAR will happily overwrite old entries when adding new entries to the archive with the same name, it will
                        // not delete an older entry when renaming a new entry to the same name. Instead, it creates a duplicate.
                        // We need to make sure that we delete the old entry to avoid creating a duplicate.

                        DeleteEntry(existingEntry);

                    }

                }

                newEntries.Add(newEntry);

                if (!options.LeaveStreamOpen)
                    stream.Close();

                return newEntries.Last();

            }
            else {

                throw new ArgumentException(Properties.ExceptionMessages.ArchiveOnlySupportsFileStreams);

            }

        }
        public override void DeleteEntry(IArchiveEntry entry) {

            if (!CanWrite)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsReadOnly);

            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            bool entryRemoved = false;

            // If this is an entry that we added previously, remove it.

            if (entry is NewArchiveEntry newArchiveEntry)
                entryRemoved = newEntries.Remove(newArchiveEntry);

            // If this is an existing entry, remove it.

            if (existingEntries.Value.Contains(entry)) {

                deletedEntries.Add(entry);

                entryRemoved = true;

            }

            if (!entryRemoved)
                throw new ArchiveEntryDoesNotExistException();

        }
        public override IEnumerable<IArchiveEntry> GetEntries() {

            if (!CanRead)
                throw new InvalidOperationException(Properties.ExceptionMessages.ArchiveIsWriteOnly);

            // Include all existing and new entries minus deleted and overwritten entries.

            return existingEntries.Value
                .Concat(newEntries)
                .Except(deletedEntries)
                .Except(existingEntries.Value.Where(entry => newEntries.Any(newEntry => newEntry.Name.Equals(entry.Name))));

        }

        public override void Close() {

            if (!archiveIsClosed)
                CommitChanges(newEntries, deletedEntries);

            archiveIsClosed = true;

        }

        // Internal members

        protected class NewArchiveEntry :
            GenericArchiveEntry {

            public string FilePath { get; set; }
            public bool RenameRequired => !Name.Equals(PathUtilities.GetFilename(FilePath));

        }

        // Protected members

        protected DeferredCreationArchiveBase() {
        }
        protected DeferredCreationArchiveBase(IArchiveOptions archiveOptions) :
            base(archiveOptions) {

            existingEntries = new Lazy<List<IArchiveEntry>>(ReadArchiveEntries);

        }

        protected abstract List<IArchiveEntry> ReadArchiveEntries();
        protected abstract void CommitChanges(IEnumerable<NewArchiveEntry> newEntries, IEnumerable<IArchiveEntry> deletedEntries);

        // Private members

        private readonly Lazy<List<IArchiveEntry>> existingEntries;
        private readonly List<NewArchiveEntry> newEntries = new List<NewArchiveEntry>();
        private readonly List<IArchiveEntry> deletedEntries = new List<IArchiveEntry>();
        private bool archiveIsClosed = false;

    }

}
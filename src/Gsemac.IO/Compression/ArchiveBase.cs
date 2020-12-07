using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public abstract class ArchiveBase :
        IArchive {

        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract string Comment { get; set; }
        public abstract CompressionLevel CompressionLevel { get; set; }

        public abstract IArchiveEntry AddEntry(Stream stream, string entryName, bool overwrite = true, bool leaveOpen = false, IArchiveEntryOptions options = null);
        public abstract IArchiveEntry GetEntry(string entryName);
        public abstract void DeleteEntry(IArchiveEntry entry);
        public abstract void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        public abstract IEnumerable<IArchiveEntry> GetEntries();

        public abstract void Close();

        public abstract void Dispose();

        public IEnumerator<IArchiveEntry> GetEnumerator() {

            return GetEntries().GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

    }

}
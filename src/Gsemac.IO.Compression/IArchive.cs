using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchive :
        IEnumerable<IArchiveEntry>,
        IDisposable {

        bool CanRead { get; }
        bool CanWrite { get; }
        string Comment { get; set; }
        CompressionLevel CompressionLevel { get; set; }

        IArchiveEntry AddEntry(Stream stream, string entryName, bool overwrite = true, bool leaveOpen = false, IArchiveEntryOptions options = null);
        IArchiveEntry GetEntry(string entryName);
        void DeleteEntry(IArchiveEntry entry);
        void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        IEnumerable<IArchiveEntry> GetEntries();

        void Close();

    }

}
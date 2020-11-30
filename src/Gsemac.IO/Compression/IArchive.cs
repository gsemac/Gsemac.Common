using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchive :
        IDisposable {

        bool CanRead { get; }
        bool CanWrite { get; }
        CompressionLevel CompressionLevel { get; set; }

        void AddEntry(Stream stream, string pathInArchive);
        IArchiveEntry GetEntry(string pathInArchive);
        void DeleteEntry(IArchiveEntry entry);
        void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        IEnumerable<IArchiveEntry> GetEntries();

        void SaveTo(Stream outputStream);

    }

}
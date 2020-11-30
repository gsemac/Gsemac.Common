using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public interface IArchive {

        void AddEntry(Stream stream, string pathInArchive);
        void RemoveEntry(IArchiveEntry entry);
        void ExtractEntry(IArchiveEntry entry, Stream outputStream);
        IEnumerable<IArchiveEntry> GetEntries();

        void SaveTo(Stream stream, Stream outputStream);

    }

}
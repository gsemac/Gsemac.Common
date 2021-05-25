using System.IO;

namespace Gsemac.IO.Compression {

    public class ArchiveEntryExistsException :
        IOException {

        public ArchiveEntryExistsException() :
            base("An entry with this name already exists in the archive.") {
        }

    }

}
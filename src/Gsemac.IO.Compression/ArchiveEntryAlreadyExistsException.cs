using System.IO;

namespace Gsemac.IO.Compression {

    public class ArchiveEntryAlreadyExistsException :
        IOException {

        public ArchiveEntryAlreadyExistsException() :
            base("An entry with this name already exists in the archive.") {
        }

    }

}
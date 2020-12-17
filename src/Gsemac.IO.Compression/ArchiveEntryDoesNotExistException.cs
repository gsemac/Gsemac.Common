using System;

namespace Gsemac.IO.Compression {

    public class ArchiveEntryDoesNotExistException :
        ArgumentException {

        public ArchiveEntryDoesNotExistException() :
            base("The entry does not exist in the archive.") {
        }

    }

}
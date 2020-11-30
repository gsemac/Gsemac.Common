#if NETFRAMEWORK45_OR_NEWER

using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.IO.Compression.Internal {

    internal class SystemIOCompressionZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Path => entry.FullName;

        public SystemIOCompressionZipArchiveEntry(System.IO.Compression.ZipArchiveEntry entry) {

            this.entry = entry;

        }

        public System.IO.Compression.ZipArchiveEntry GetEntry() {

            return entry;

        }

        // Private members

        private readonly System.IO.Compression.ZipArchiveEntry entry;

    }

}

#endif
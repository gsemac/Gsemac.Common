#if NETFRAMEWORK45_OR_NEWER

using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.IO.Compression.Implementations.Internal {

    internal class SystemIOCompressionZipArchiveEntry :
        IArchiveEntry {

        // Public members

        public string Path => BaseEntry.FullName;
        
        public System.IO.Compression.ZipArchiveEntry BaseEntry { get; }

        public SystemIOCompressionZipArchiveEntry(System.IO.Compression.ZipArchiveEntry entry) {

            this.BaseEntry = entry;

        }

    }

}

#endif
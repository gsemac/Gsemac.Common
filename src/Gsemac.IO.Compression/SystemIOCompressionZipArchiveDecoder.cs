#if NET45_OR_NEWER

using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public class SystemIOCompressionZipArchiveDecoder :
        PluginBase,
        IArchiveDecoder {

        // Public members

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new[] {
                ArchiveFormat.Zip
            };

        }

        public IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new SystemIOCompressionZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}

#endif
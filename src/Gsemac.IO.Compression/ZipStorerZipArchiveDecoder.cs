using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    public class ZipStorerZipArchiveDecoder :
        PluginBase,
        IArchiveDecoder {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => new[] {
            ArchiveFormat.Zip
        };

        public ZipStorerZipArchiveDecoder() :
            base(1) {
        }

        public IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new ZipStorerZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}
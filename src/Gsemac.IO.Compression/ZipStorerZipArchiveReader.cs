using Gsemac.Reflection.Plugins;
using System.IO;

namespace Gsemac.IO.Compression {

    public class ZipStorerZipArchiveReader :
        PluginBase,
        IArchiveReader {

        // Public members

        public ZipStorerZipArchiveReader() :
            base(1) {
        }

        public IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new ZipStorerZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}
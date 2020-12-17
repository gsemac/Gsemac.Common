using Gsemac.Reflection.Plugins;
using System.IO;

namespace Gsemac.IO.Compression {

    public class SharpCompressZipArchiveReader :
        PluginBase,
        IArchiveReader {

        // Public members

        public IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new SharpCompressZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}
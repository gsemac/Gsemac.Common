#if NET45_OR_NEWER

using Gsemac.Reflection.Plugins;
using System.IO;

namespace Gsemac.IO.Compression {

    public class NetFrameworkZipArchiveReader :
        PluginBase,
        IArchiveReader {

        // Public members

        public IArchive OpenStream(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new NetFrameworkZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}

#endif
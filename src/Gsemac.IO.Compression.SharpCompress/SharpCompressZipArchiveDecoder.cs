using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using System.IO;

namespace Gsemac.IO.Compression {

    [RequiresAssemblyOrType("SharpCompress", "SharpCompress.Archives.Zip.ZipArchive")]
    public class SharpCompressZipArchiveDecoder :
        PluginBase,
        IArchiveDecoder {

        // Public members

        public IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new SharpCompressZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}
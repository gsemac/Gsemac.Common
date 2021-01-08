using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    [RequiresAssemblyOrType("SharpCompress", "SharpCompress.Archives.SevenZip.SevenZipArchive")]
    public class SharpCompress7ZipArchiveDecoder :
        PluginBase,
        IArchiveDecoder {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => new[] {
            ArchiveFormat.SevenZip
        };

        public IArchive Decode(Stream stream, FileAccess fileAccess = FileAccess.ReadWrite, bool leaveOpen = false, IArchiveOptions options = null) {

            return new SharpCompress7ZipArchive(stream, fileAccess, leaveOpen, options);

        }

    }

}
using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    [RequiresAssemblyOrTypes("SharpCompress", "SharpCompress.Archives.SevenZip.SevenZipArchive", "SharpCompress.Archives.Zip.ZipArchive")]
    public class SharpCompressArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            List<ICodecCapabilities> codecCapabilities = new List<ICodecCapabilities> {
                new CodecCapabilities(ArchiveFormat.Zip, canRead: true, canWrite: true),
                new CodecCapabilities(ArchiveFormat.SevenZip, canRead: true, canWrite: false),
            };

            return codecCapabilities.OrderBy(f => f.Format.Extensions.First());

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new UnsupportedFileFormatException(archiveFormat);

            if (archiveFormat.Equals(ArchiveFormat.Zip)) {

                return new SharpCompressSevenZipArchive(stream, archiveOptions);

            }
            else if (archiveFormat.Equals(ArchiveFormat.SevenZip)) {

                return new SharpCompressSevenZipArchive(stream, archiveOptions);

            }
            else {

                throw new UnsupportedFileFormatException(archiveFormat);

            }

        }

    }

}
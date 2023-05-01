using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    [RequiresSevenZip]
    public class BinSevenZipArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public BinSevenZipArchiveFactory() :
            this(ArchiveFactoryOptions.Default) {
        }
        public BinSevenZipArchiveFactory(IArchiveFactoryOptions options) {

            this.options = options;

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return new IFileFormat[] {
                ArchiveFormat.Zip,
                ArchiveFormat.SevenZip,
            }
            .OrderBy(f => f.Extensions.First())
            .Distinct()
            .Select(f => new CodecCapabilities(f, canRead: true, canWrite: true));

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new UnsupportedFileFormatException(archiveFormat);

            return new BinSevenZipArchive(stream, options.SevenZipDirectoryPath, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly IArchiveFactoryOptions options;

    }

}
using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

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

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new IFileFormat[] {
                new ZipFileFormat(),
                new SevenZipFileFormat(),
            };

        }
        public IEnumerable<IFileFormat> GetWritableFileFormats() {

            return GetSupportedFileFormats();

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
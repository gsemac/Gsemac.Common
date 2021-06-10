using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression.SevenZip {

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

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return new BinSevenZipArchive(stream, options.SevenZipExecutablePath, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly IArchiveFactoryOptions options;

    }

}
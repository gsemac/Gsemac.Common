using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression.Winrar {

    [RequiresWinrar]
    public class WinrarArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public WinrarArchiveFactory() :
            this(ArchiveFactoryOptions.Default) {
        }
        public WinrarArchiveFactory(IArchiveFactoryOptions options) {

            this.options = options;

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new IFileFormat[] {
                new ZipFileFormat(),
                new RarFileFormat(),
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

            return new BinWinrarArchive(stream, options.WinrarDirectoryPath, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly IArchiveFactoryOptions options;

    }

}
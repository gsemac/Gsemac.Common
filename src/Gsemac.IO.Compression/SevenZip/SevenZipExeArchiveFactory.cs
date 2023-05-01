using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.SevenZip {

    [RequiresSevenZipExe]
    public class SevenZipExeArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public SevenZipExeArchiveFactory() :
            this(SevenZipExeArchiveFactoryOptions.Default) {
        }
        public SevenZipExeArchiveFactory(ISevenZipExeArchiveFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.options = options;

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            // TODO: Review formats supported by 7-Zip.
            // https://superuser.com/questions/1105516/comparing-7z-exe-and-7za-exe

            return new IFileFormat[] {
                ArchiveFormat.Zip,
                ArchiveFormat.SevenZip,
            }
            .OrderBy(f => f.Extensions.First())
            .Distinct()
            .Select(f => new CodecCapabilities(f, canRead: true, canWrite: true));

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new UnsupportedFileFormatException(archiveFormat);

            return new SevenZipExeArchive(stream, options.SevenZipDirectoryPath, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly ISevenZipExeArchiveFactoryOptions options;

    }

}
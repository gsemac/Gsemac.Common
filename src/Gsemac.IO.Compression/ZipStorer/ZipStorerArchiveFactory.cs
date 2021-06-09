using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression.ZipStorer {

    public sealed class ZipStorerArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public ZipStorerArchiveFactory() :
            base(1) {
        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new IFileFormat[] {
               new ZipFileFormat(),
           };

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return new ZipStorerZipArchive(stream, archiveOptions);

        }

    }

}
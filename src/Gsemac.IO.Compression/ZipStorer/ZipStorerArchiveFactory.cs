using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression.ZipStorer {

    [PluginPriority(Priority.High)]
    public sealed class ZipStorerArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

        public ZipStorerArchiveFactory() { }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return new IFileFormat[] {
              ArchiveFormat.Zip,
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

            return new ZipStorerArchive(stream, archiveOptions);

        }

    }

}
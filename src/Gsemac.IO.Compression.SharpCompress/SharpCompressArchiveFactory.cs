using Gsemac.IO.Extensions;
using Gsemac.Reflection;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression {

    [RequiresAssemblyOrTypes("SharpCompress", "SharpCompress.Archives.SevenZip.SevenZipArchive", "SharpCompress.Archives.Zip.ZipArchive")]
    public class SharpCompressArchiveFactory :
        PluginBase,
        IArchiveFactory {

        // Public members

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

            if (archiveFormat.Equals(ArchiveFormat.Zip))
                return new SharpCompressSevenZipArchive(stream, archiveOptions.FileAccess, archiveOptions.LeaveStreamOpen, archiveOptions);
            else if (archiveFormat.Equals(ArchiveFormat.SevenZip))
                return new SharpCompressSevenZipArchive(stream, archiveOptions.FileAccess, archiveOptions.LeaveStreamOpen, archiveOptions);
            else
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

        }

    }

}
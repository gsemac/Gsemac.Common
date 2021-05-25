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

        public IArchive Open(Stream stream, IFileFormat archiveFormat = null, IArchiveOptions archiveOptions = null) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new FileFormatException(Properties.ExceptionMessages.UnsupportedFileFormat);

            if (archiveFormat.Equals(new ZipFileFormat()))
                return new SharpCompress7ZipArchive(stream, archiveOptions.FileAccess, archiveOptions.LeaveStreamOpen, archiveOptions);
            else if (archiveFormat.Equals(new SevenZipFileFormat()))
                return new SharpCompress7ZipArchive(stream, archiveOptions.FileAccess, archiveOptions.LeaveStreamOpen, archiveOptions);
            else
                throw new FileFormatException(Properties.ExceptionMessages.UnsupportedFileFormat);

        }

    }

}
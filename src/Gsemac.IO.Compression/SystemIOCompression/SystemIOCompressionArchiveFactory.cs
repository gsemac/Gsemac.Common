#if NET45_OR_NEWER

using Gsemac.IO.Extensions;
using Gsemac.IO.FileFormats;
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.IO.Compression.SystemIOCompression {

    public sealed class SystemIOCompressionArchiveFactory :
           PluginBase,
           IArchiveFactory {

        // Public members

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return new IFileFormat[] {
               ArchiveFormat.Zip,
           };

        }
        public IEnumerable<IFileFormat> GetWritableFileFormats() {

            return GetSupportedFileFormats();

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat = null, IArchiveOptions archiveOptions = null) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (!this.IsSupportedFileFormat(archiveFormat))
                throw new UnsupportedFileFormatException(archiveFormat);

            return new SystemIOCompressionArchive(stream, archiveOptions);

        }

    }

}

#endif
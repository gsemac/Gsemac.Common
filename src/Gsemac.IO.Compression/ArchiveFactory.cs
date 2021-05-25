using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public class ArchiveFactory :
        IArchiveFactory {

        // Public members

        public static ArchiveFactory Default => new ArchiveFactory();

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedArchiveFormats();

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat = null, IArchiveOptions archiveOptions = null) {

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            IArchiveFactory archiveFactory = GetArchiveFactoryForFormat(archiveFormat);

            if (archiveFactory is null)
                throw new FileFormatException(Properties.ExceptionMessages.UnsupportedFileFormat);

            return archiveFactory.Open(stream, archiveFormat, archiveOptions);

        }

        // Private members

        private static IEnumerable<IFileFormat> GetSupportedArchiveFormats() {

            return CompressionPluginLoader.GetArchiveFactories().SelectMany(decoder => decoder.GetSupportedFileFormats())
                .OrderBy(type => type)
                .Distinct();

        }
        private static IArchiveFactory GetArchiveFactoryForFormat(IFileFormat fileFormat) {

            return CompressionPluginLoader.GetArchiveFactories().FirstOrDefault(decoder => decoder.IsSupportedFileFormat(fileFormat));

        }

    }

}
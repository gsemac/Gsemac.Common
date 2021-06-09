using Gsemac.IO.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Compression {

    public class ArchiveFactory :
        IArchiveFactory {

        // Public members

        public static ArchiveFactory Default => new ArchiveFactory();

        public ArchiveFactory() :
            this(ArchiveFactoryOptions.Default) {

        }
        public ArchiveFactory(IArchiveFactoryOptions options) {

            this.options = options;

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedArchiveFormats();

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            IArchiveFactory archiveFactory = GetArchiveFactoryForFormat(archiveFormat);

            if (archiveFactory is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return archiveFactory.Open(stream, archiveFormat, archiveOptions);

        }

        // Private members

        private readonly IArchiveFactoryOptions options;

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
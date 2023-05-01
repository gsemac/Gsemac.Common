using Gsemac.Reflection.Plugins;
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
            this(null) {

        }
        public ArchiveFactory(IServiceProvider serviceProvider) {

            pluginLoader = CreatePluginLoader(serviceProvider);

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return GetSupportedArchiveFormats();

        }

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            if (archiveFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            List<Exception> exceptions = new List<Exception>();

            // Get an archive factory capable of opening this archive according to the file access option.

            var a = GetArchiveFactories();

            var b = a.SelectMany(f => f.GetSupportedFileFormats());

            IArchiveFactory archiveFactory = GetArchiveFactories()
                .Where(decoder => decoder.GetSupportedFileFormats()
                    .Where(f => (archiveOptions.FileAccess == FileAccess.Write && f.CanWrite) || f.CanRead)
                    .Any(f => f.Format.Equals(archiveFormat)))
                .FirstOrDefault();

            if (archiveFactory is object) {

                return archiveFactory.Open(stream, archiveFormat, archiveOptions);

            }
            else {

                // If we reach this point, we have no factories capable of reading this file format.

                throw new FileFormatException(archiveFormat is null ? IO.Properties.ExceptionMessages.UnsupportedFileFormat : string.Format(IO.Properties.ExceptionMessages.UnsupportedFileFormatWithFormat, archiveFormat));

            }

        }

        // Private members

        private readonly IPluginLoader pluginLoader;

        private IEnumerable<IArchiveFactory> GetArchiveFactories() {

            return pluginLoader.GetPlugins<IArchiveFactory>();

        }
        private IEnumerable<ICodecCapabilities> GetSupportedArchiveFormats() {

            return CodecCapabilities.Flatten(GetArchiveFactories().SelectMany(decoder => decoder.GetSupportedFileFormats()))
                .OrderBy(format => format.Format);

        }

        private IPluginLoader CreatePluginLoader(IServiceProvider serviceProvider) {

            return new PluginLoader<IArchiveFactory>(serviceProvider, new PluginLoaderOptions() {
                PluginSearchPattern = "Gsemac.IO.Compression.*.dll",
            });

        }

    }

}
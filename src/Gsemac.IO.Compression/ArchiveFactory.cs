using Gsemac.IO.Extensions;
using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
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
            this(ArchiveFactoryOptions.Default) {

        }
        public ArchiveFactory(IPluginLoader pluginLoader) :
            this(pluginLoader, ArchiveFactoryOptions.Default) {
        }
        public ArchiveFactory(IArchiveFactoryOptions options) :
            this(null, options) {
        }
        public ArchiveFactory(IPluginLoader pluginLoader, IArchiveFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (pluginLoader is null)
                this.pluginLoader = new Lazy<IPluginLoader>(CreateDefaultPluginLoader);
            else
                this.pluginLoader = new Lazy<IPluginLoader>(() => pluginLoader);

            this.options = options;

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedArchiveFormats();

        }
        public IEnumerable<IFileFormat> GetWritableFileFormats() {

            return GetSupportedWritableArchiveFormats();

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

            IArchiveFactory archiveFactory = GetArchiveFactories()
                .Where(decoder => (archiveOptions.FileAccess.HasFlag(FileAccess.Write) ? decoder.GetWritableFileFormats() : decoder.GetSupportedFileFormats()).Any(format => format.Equals(archiveFormat)))
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

        private readonly Lazy<IPluginLoader> pluginLoader;
        private readonly IArchiveFactoryOptions options;

        private IEnumerable<IArchiveFactory> GetArchiveFactories() {

            return pluginLoader.Value.GetPlugins<IArchiveFactory>();

        }
        private IEnumerable<IFileFormat> GetSupportedArchiveFormats() {

            return GetArchiveFactories().SelectMany(decoder => decoder.GetSupportedFileFormats())
                .OrderBy(type => type)
                .Distinct();

        }
        private IEnumerable<IFileFormat> GetSupportedWritableArchiveFormats() {

            return GetArchiveFactories().SelectMany(decoder => decoder.GetWritableFileFormats())
                .OrderBy(type => type)
                .Distinct();

        }

        private IPluginLoader CreateDefaultPluginLoader() {

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(options)
                .BuildServiceProvider();

            return new PluginLoader<IArchiveFactory>(serviceProvider, new PluginLoaderOptions() {
                PluginSearchPattern = "Gsemac.IO.Compression.*.dll",
            });

        }

    }

}
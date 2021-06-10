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

        public IArchive Open(Stream stream, IFileFormat archiveFormat, IArchiveOptions archiveOptions) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (archiveFormat is null)
                stream = FileFormatFactory.Default.FromStream(stream, out archiveFormat);

            if (archiveOptions is null)
                archiveOptions = ArchiveOptions.Default;

            List<Exception> exceptions = new List<Exception>();

            foreach (IArchiveFactory archiveFactory in GetArchiveFactories().Where(decoder => decoder.IsSupportedFileFormat(archiveFormat))) {

                try {

                    return archiveFactory.Open(stream, archiveFormat, archiveOptions);

                }
                catch (NotSupportedException ex) {

                    // We may get this exception if we try to open the archive with a factory that doesn't support the file access specified.
                    // For example, some factories only support read access.
                    // By catching the exception and trying again with the next factory, we can find a factory that supports the desired access.

                    exceptions.Add(ex);

                }

            }

            // If we reach this point, we have no factories capable of reading this file format.

            if (exceptions.Any())
                throw new AggregateException(exceptions);

            throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

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
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageOptimizerFactory :
        IImageOptimizerFactory {

        // Public members

        public static ImageOptimizerFactory Default => new ImageOptimizerFactory();

        public ImageOptimizerFactory() :
           this(null) {
        }
        public ImageOptimizerFactory(IPluginLoader pluginLoader) {

            if (pluginLoader is null)
                this.pluginLoader = new Lazy<IPluginLoader>(CreateDefaultPluginLoader);
            else
                this.pluginLoader = new Lazy<IPluginLoader>(() => pluginLoader);

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImageOptimizer FromFileFormat(IFileFormat imageFormat) {

            return GetImageOptimizers().FirstOrDefault(optimizer => optimizer.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private readonly Lazy<IPluginLoader> pluginLoader;

        private IPluginLoader CreateDefaultPluginLoader() {

            return new PluginLoader<IImageOptimizer>(new PluginLoaderOptions() {
                PluginSearchPattern = "Gsemac.Drawing.Imaging.*.dll",
            });

        }
        private IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return GetImageOptimizers().SelectMany(optimizer => optimizer.GetSupportedFileFormats())
                .OrderBy(type => type)
                .Distinct();

        }
        private IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return GetImageOptimizersInternal();

        }
        private IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            return pluginLoader.Value.GetPlugins<IImageOptimizer>();

        }

    }

}
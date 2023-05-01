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
        public ImageOptimizerFactory(IServiceProvider serviceProvider) {

            pluginLoader = CreatePluginLoader(serviceProvider);

        }

        public IEnumerable<ICodecCapabilities> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImageOptimizer FromFileFormat(IFileFormat imageFormat) {

            return GetImageOptimizers().FirstOrDefault(optimizer => optimizer.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private readonly IPluginLoader pluginLoader;

        private IPluginLoader CreatePluginLoader(IServiceProvider serviceProvider) {

            return new PluginLoader<IImageOptimizer>(serviceProvider, new PluginLoaderOptions() {
                PluginSearchPattern = "Gsemac.Drawing.Imaging.*.dll",
            });

        }
        private IEnumerable<ICodecCapabilities> GetSupportedImageFormats() {

            return CodecCapabilities.Flatten(GetImageOptimizers().SelectMany(optimizer => optimizer.GetSupportedFileFormats()))
                .OrderBy(type => type);

        }
        private IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return GetImageOptimizersInternal();

        }
        private IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            return pluginLoader.GetPlugins<IImageOptimizer>();

        }

    }

}
using Gsemac.Reflection.Plugins;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    internal static class ImagingPluginLoader {

        // Public members

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            return pluginLoader.GetPlugins<IImageCodec>();

        }
        public static IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return pluginLoader.GetPlugins<IImageOptimizer>();

        }

        // Private members

        private static readonly IPluginLoader pluginLoader = new PluginLoader("Gsemac.Drawing.Imaging.*.dll");

    }

}
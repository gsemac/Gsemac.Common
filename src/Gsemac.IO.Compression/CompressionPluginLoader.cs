using Gsemac.Reflection.Plugins;
using System.Collections.Generic;

namespace Gsemac.IO.Compression {

    internal static class CompressionPluginLoader {

        // Public members

        public static IEnumerable<IArchiveFactory> GetArchiveFactories() {

            return pluginLoader.GetPlugins<IArchiveFactory>();

        }

        // Private members

        private static readonly IPluginLoader pluginLoader = new PluginLoader("Gsemac.IO.Compression.*.dll");

    }

}
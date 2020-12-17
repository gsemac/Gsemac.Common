using Gsemac.Reflection.Plugins;
using System.Collections.Generic;

namespace Gsemac.IO.Compression {

    public static class CompressionPluginLoader {

        // Public members

        public static IEnumerable<IArchiveReader> GetArchiveReaders() {

            return pluginLoader.GetPlugins<IArchiveReader>();

        }

        // Private members

        private static readonly IPluginLoader pluginLoader = new PluginLoader("Gsemac.IO.Compression.*.dll");

    }

}
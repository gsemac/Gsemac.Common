using Gsemac.Reflection.Plugins;
using System.Collections.Generic;

namespace Gsemac.IO.Compression {

    internal static class CompressionPluginLoader {

        // Public members

        public static IEnumerable<IArchiveDecoder> GetArchiveDecoders() {

            return pluginLoader.GetPlugins<IArchiveDecoder>();

        }

        // Private members

        private static readonly IPluginLoader pluginLoader = new PluginLoader("Gsemac.IO.Compression.*.dll");

    }

}
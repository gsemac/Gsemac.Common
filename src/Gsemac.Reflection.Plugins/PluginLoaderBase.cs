using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public abstract class PluginLoaderBase :
        IPluginLoader {

        // Public members

        public abstract IEnumerable<IPlugin> GetPlugins();
        public IEnumerable<T> GetPlugins<T>() {

            return GetPlugins().OfType<T>();

        }

    }

}
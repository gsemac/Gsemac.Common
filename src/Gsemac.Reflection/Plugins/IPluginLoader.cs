using System.Collections.Generic;

namespace Gsemac.Reflection.Plugins {

    public interface IPluginLoader {

        IEnumerable<IPlugin> GetPlugins();
        IEnumerable<T> GetPlugins<T>();

    }

}
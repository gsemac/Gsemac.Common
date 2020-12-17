using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public class PluginLoader :
        IPluginLoader {

        // Public members

        public PluginLoader(string pluginSearchPattern) :
            this(AssemblyResolver.Default as IFileSystemAssemblyResolver, pluginSearchPattern) {
        }
        public PluginLoader(IFileSystemAssemblyResolver assemblyResolver) :
            this(assemblyResolver, string.Empty) {

            if (assemblyResolver is null)
                throw new ArgumentNullException(nameof(assemblyResolver));

        }
        public PluginLoader(IFileSystemAssemblyResolver assemblyResolver, string pluginSearchPattern) {

            this.assemblyResolver = assemblyResolver ?? new FileSystemAssemblyResolver();
            this.pluginSearchPattern = pluginSearchPattern;

        }

        public IEnumerable<IPlugin> GetPlugins() {

            lock (pluginsLoadLock) {

                LoadPluginAssemblies();
                LoadPlugins();

                return plugins.Where(plugin => TypeUtilities.TestRequirementAttributes(plugin.GetType()));

            }

        }
        public IEnumerable<T> GetPlugins<T>() {

            return GetPlugins().OfType<T>();

        }

        // Protected members

        protected virtual IEnumerable<IPlugin> GetPluginsInternal() {

            return TypeUtilities.GetTypesImplementingInterface<IPlugin>()
                .Where(type => type.IsDefaultConstructable())
                .Select(type => (IPlugin)Activator.CreateInstance(type));

        }

        // Private members

        private readonly IFileSystemAssemblyResolver assemblyResolver;
        private readonly string pluginSearchPattern;
        private bool assembliesLoaded = false;
        private bool pluginsLoaded = false;

        private static IEnumerable<IPlugin> plugins = Enumerable.Empty<IPlugin>();
        private static readonly object pluginsLoadLock = new object();

        private void LoadPluginAssemblies() {

            if (assembliesLoaded)
                return;

            if (!string.IsNullOrWhiteSpace(pluginSearchPattern)) {

                IEnumerable<string> assemblyPaths = string.IsNullOrEmpty(pluginSearchPattern) ?
                       assemblyResolver.GetAssemblyPaths() :
                       assemblyResolver.GetAssemblyPaths(pluginSearchPattern);

                foreach (string filename in assemblyPaths)
                    AppDomain.CurrentDomain.Load(File.ReadAllBytes(filename));

                assembliesLoaded = true;

            }

        }
        private void LoadPlugins() {

            if (pluginsLoaded)
                return;

            // All PluginLoader instances share the same set of plugins, which prevents the same plugins from being instantiated more than once.

            plugins = plugins.Concat(GetPluginsInternal()
                .Where(plugin => !plugins.Any(p => p.GetType().Equals(plugin.GetType()))))
                .OrderByDescending(plugin => plugin.Priority)
                .ToArray();

            pluginsLoaded = true;

        }

    }

}
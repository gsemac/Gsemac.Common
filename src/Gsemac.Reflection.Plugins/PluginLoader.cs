﻿using Gsemac.Polyfills.Microsoft.Extensions.DependencyInjection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Reflection.Plugins {

    public class PluginLoader<PluginT> :
        PluginLoaderBase {

        // Public members

        public PluginLoader() :
            this((IServiceProvider)null) {
        }
        public PluginLoader(IServiceProvider serviceProvider) :
            this(serviceProvider, PluginLoaderOptions.Default) {
        }
        public PluginLoader(IPluginLoaderOptions options) :
           this(null, options) {
        }
        public PluginLoader(IServiceProvider serviceProvider, IPluginLoaderOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            this.serviceProvider = serviceProvider ?? new ServiceCollection().BuildServiceProvider();
            this.options = options;
            this.plugins = new Lazy<IEnumerable<IPlugin>>(InitializePlugins);

        }

        public override IEnumerable<IPlugin> GetPlugins() {

            return plugins.Value.Where(plugin => PluginUtilities.TestRequirementAttributes(plugin.GetType(), serviceProvider));

        }

        // Private members

        private readonly IServiceProvider serviceProvider;
        private readonly IPluginLoaderOptions options;
        private readonly Lazy<IEnumerable<IPlugin>> plugins;

        private static readonly object globalMutex = new object();
        private static readonly HashSet<string> globalLoadedAssemblies = new HashSet<string>();

        private IEnumerable<IPlugin> InitializePlugins() {

            LoadPluginAssemblies();

            IEnumerable<Type> pluginTypes = TypeUtilities.GetTypesImplementingInterface<IPlugin>()
                .Where(type => typeof(PluginT).IsAssignableFrom(type));

            // Instantiate all of the relevant plugins.
            // All plugins that cannot be loaded successfully will be silently ignored.

            return pluginTypes.Where(type => CanConstructObjectOfType(type))
                .Where(type => !IsPluginIgnored(type))
                .OrderByDescending(type => GetPluginPriority(type))
                .Select(type => TryCreateInstance(type))
                .Where(obj => obj is object)
                .Cast<IPlugin>()
                .ToArray();

        }
        private void LoadPluginAssemblies() {

            if (!string.IsNullOrWhiteSpace(options.PluginSearchPattern)) {

                IEnumerable<string> assemblyPaths = string.IsNullOrEmpty(options.PluginSearchPattern) ?
                       options.AssemblyResolver.GetAssemblyPaths() :
                       options.AssemblyResolver.GetAssemblyPaths(options.PluginSearchPattern);

                lock (globalMutex) {

                    // Attempt to load all assemblies relevant to this plugin loader.
                    // Any assemblies that cannot be loaded successfully will be silently ignored.

                    foreach (string assemblyPath in assemblyPaths.Where(assemblyPath => !globalLoadedAssemblies.Contains(assemblyPath)).ToArray()) {

                        try {

                            AppDomain.CurrentDomain.Load(File.ReadAllBytes(assemblyPath));

                        }
                        catch (Exception) {
#if DEBUG
                            throw;
#endif
                        }

                        globalLoadedAssemblies.Add(assemblyPath);

                    }

                }

            }

        }
        private object TryCreateInstance(Type type) {

            try {

                return serviceProvider is object ? ActivatorUtilities.CreateInstance(serviceProvider, type) : Activator.CreateInstance(type);

            }
            catch (Exception) {
#if DEBUG
                throw;
#else
                return null;
#endif
            }

        }

        private bool IsPluginIgnored(Type type) {

            return type.HasAttribute<IgnorePluginAttribute>(inherit: false);

        }
        private int GetPluginPriority(Type type) {

            return type.GetCustomAttributes(inherit: false)
                .OfType<PluginPriorityAttribute>()
                .FirstOrDefault()?.Priority ?? 0;

        }
        private bool CanConstructObjectOfType(Type type) {

            return serviceProvider is object || type.IsDefaultConstructable();

        }

    }

    public class PluginLoader :
        PluginLoader<IPlugin> {

        // Public members

        public PluginLoader() {
        }
        public PluginLoader(IServiceProvider serviceProvider) :
            base(serviceProvider) {
        }
        public PluginLoader(IPluginLoaderOptions options) :
            base(options) {
        }
        public PluginLoader(IServiceProvider serviceProvider, IPluginLoaderOptions options) :
            base(serviceProvider, options) {
        }

    }

}
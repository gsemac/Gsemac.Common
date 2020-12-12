using Gsemac.Drawing.Imaging;
using Gsemac.Reflection;
using Gsemac.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Internal {

    internal static class PluginLoader {

        // Public members

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            return imageCodecs.Value
                .Where(codec => TypeUtilities.TestRequirementAttributes(codec.GetType()));

        }
        public static IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return imageOptimizers.Value
                .Where(optimizer => TypeUtilities.TestRequirementAttributes(optimizer.GetType()));

        }

        // Private members

        private static bool drawingImagingAssembliesLoaded = false;
        private static readonly object drawingImagingAssemblyLoadLock = new object();

        private static readonly Lazy<IEnumerable<IImageCodec>> imageCodecs = new Lazy<IEnumerable<IImageCodec>>(GetImageCodecsInternal);
        private static readonly Lazy<IEnumerable<IImageOptimizer>> imageOptimizers = new Lazy<IEnumerable<IImageOptimizer>>(GetImageOptimizersInternal);

        private static void LoadPluginAssemblies() {

            if (drawingImagingAssembliesLoaded)
                return;

            lock (drawingImagingAssemblyLoadLock) {

                if (drawingImagingAssembliesLoaded)
                    return;

                // Load all Gsemac.Drawing.Plugins.* assemblies.

                FileSystemAssemblyResolver assemblyResolver = new FileSystemAssemblyResolver();

                foreach (string filename in assemblyResolver.GetAssemblyPaths("Gsemac.Drawing.Imaging.*.dll"))
                    AppDomain.CurrentDomain.Load(File.ReadAllBytes(filename));

                drawingImagingAssembliesLoaded = true;

            }

        }

        private static IEnumerable<IImageCodec> GetImageCodecsInternal() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageCodec>()
                .Where(type => type.IsDefaultConstructable())
                .Select(type => (IImageCodec)Activator.CreateInstance(type));

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageOptimizer>()
                .Where(type => type.IsDefaultConstructable())
                .Select(type => (IImageOptimizer)Activator.CreateInstance(type));

        }

    }

}
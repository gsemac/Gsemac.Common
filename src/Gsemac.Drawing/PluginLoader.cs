using Gsemac.Core;
using Gsemac.Drawing.Imaging;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gsemac.Drawing.Internal {

    internal static class PluginLoader {

        // Public members

        public static IEnumerable<Type> GetImageCodecs() {

            return imageCodecTypes.Value;

        }
        public static IEnumerable<Type> GetImageOptimizers() {

            return imageOptimizerTypes.Value;

        }

        // Private members

        private static bool drawingImagingAssembliesLoaded = false;
        private static readonly object drawingImagingAssemblyLoadLock = new object();

        private static readonly Lazy<IEnumerable<Type>> imageCodecTypes = new Lazy<IEnumerable<Type>>(GetImageCodecsInternal);
        private static readonly Lazy<IEnumerable<Type>> imageOptimizerTypes = new Lazy<IEnumerable<Type>>(GetImageOptimizersInternal);

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

        private static IEnumerable<Type> GetImageCodecsInternal() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageCodec>();

        }
        private static IEnumerable<Type> GetImageOptimizersInternal() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageOptimizer>();

        }

    }

}
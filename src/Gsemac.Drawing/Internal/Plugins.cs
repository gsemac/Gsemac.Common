using Gsemac.Core;
using Gsemac.Drawing.Imaging;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Internal {

    // #todo Proper plugin architecture

    internal static class Plugins {

        // Public members

        public static IEnumerable<Type> GetImageCodecs() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageCodec>();

        }
        public static IEnumerable<Type> GetImageOptimizers() {

            LoadPluginAssemblies();

            return TypeUtilities.GetTypesImplementingInterface<IImageOptimizer>();

        }

        // Private members

        private static bool drawingImagingAssembliesLoaded = false;
        private static readonly object drawingImagingAssemblyLoadLock = new object();

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

    }

}
using Gsemac.Reflection;
using System;
using System.Linq;

namespace Gsemac.Drawing.Imaging.Internal {

    // #todo Proper plugin architecture

    internal static class Plugins {

        // Public members

        public static Lazy<bool> IsWebPWrapperAvailable { get; } = new Lazy<bool>(GetIsWebPWrapperAvailable);
        public static Lazy<bool> IsImageMagickAvailable { get; } = new Lazy<bool>(GetIsImageMagickAvailable);

        // Private members

        private static bool GetIsWebPWrapperAvailable() {

#if NETFRAMEWORK

            AnyCpuFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver();

            // Check for the presence of the "WebPWrapper.WebP" class (in case something like ilmerge was used and the assembly is not present on disk).

            bool webPWrapperExists = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType("WebPWrapper.WebP") != null)
                .FirstOrDefault();

            // Check for WebPWrapper on disk.

            if (!webPWrapperExists)
                webPWrapperExists = assemblyResolver.AssemblyExists("WebPWrapper");

            // Check for libwebp on disk.

            bool libWebPExists = assemblyResolver.AssemblyExists(Environment.Is64BitProcess ? "libwebp_x64" : "libwebp_x86");

            return webPWrapperExists && libWebPExists;

#else

            return false;

#endif

        }
        private static bool GetIsImageMagickAvailable() {

            AnyCpuFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver();

            // Check for the presence of the "ImageMagick.MagickImage" class (in case something like ilmerge was used and the assembly is not present on disk).

            bool isAvailable = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType("ImageMagick.MagickImage") != null)
                .FirstOrDefault();

            // Check for ImageMagick on disk.

            if (!isAvailable)
                isAvailable = assemblyResolver.AssemblyExists("Magick.NET-Q16-AnyCPU");

            return isAvailable;

        }

    }

}
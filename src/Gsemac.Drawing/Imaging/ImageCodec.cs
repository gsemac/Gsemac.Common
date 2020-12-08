#if NETFRAMEWORK

using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageCodec {

        // Public members

        public static IEnumerable<string> SupportedFileTypes => GetSupportedFileTypes();
        public static IEnumerable<string> NativelySupportedFileTypes => GetNativelySupportedFileTypes();

        public static bool IsSupportedFileType(string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return GetSupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }
        public static bool IsNativelySupportedFileType(string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return GetNativelySupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            List<IImageCodec> imageReaders = new List<IImageCodec> {
                new NativeImageCodec(),
            };

            if (IsWebPSupportAvailable.Value)
                imageReaders.Add(new WebPImageCodec());

            return imageReaders;

        }

        public static IImageCodec FromFileExtension(string filePath) {

            return GetImageCodecs().FirstOrDefault(codec => codec.IsSupportedFileType(filePath));

        }

        // Private members

        private static Lazy<bool> IsWebPSupportAvailable { get; } = new Lazy<bool>(GetIsWebPSupportAvailable);

        private static bool GetIsWebPSupportAvailable() {

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

        }

        private static IEnumerable<string> GetSupportedFileTypes() {

            return GetImageCodecs().SelectMany(codec => codec.SupportedFileTypes);

        }
        private static IEnumerable<string> GetNativelySupportedFileTypes() {

            List<string> extensions = new List<string>(new[]{
                ".bmp",
                ".gif",
                ".exif",
                ".jpg",
                ".jpeg",
                ".png",
                ".tif",
                ".tiff"
            });

            return extensions;

        }

    }

}

#endif
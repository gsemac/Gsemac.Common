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

        public static bool IsSupportedFileType(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return GetSupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }
        public static bool IsNativelySupportedFileType(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return GetNativelySupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(string.Empty);

        }

        public static IImageCodec FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            return GetImageCodecs(ext).FirstOrDefault(codec => codec.IsSupportedFileType(filePath));

        }

        // Private members

#if NETFRAMEWORK

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

#endif

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
        private static IEnumerable<IImageCodec> GetImageCodecs(string fileExtension) {

            List<IImageCodec> imageCodecs = new List<IImageCodec>();

#if NETFRAMEWORK

            if (string.IsNullOrEmpty(fileExtension))
                imageCodecs.Add(new GdiImageCodec());
            else if (IsNativelySupportedFileType(fileExtension))
                imageCodecs.Add(new GdiImageCodec(ImageFormat.FromFileExtension(fileExtension)));

            if (IsWebPSupportAvailable.Value)
                imageCodecs.Add(new WebPImageCodec());

#endif

            return imageCodecs;

        }

    }

}
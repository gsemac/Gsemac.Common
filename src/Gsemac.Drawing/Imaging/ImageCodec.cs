using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageCodec {

        // Public members

        public static IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();
        public static IEnumerable<IImageFormat> NativelySupportedImageFormats => GetNativelySupportedImageFormats();

        public static bool IsSupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return IsSupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(IImageFormat imageFormat) {

            return SupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }
        public static bool IsNativelySupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return IsNativelySupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsNativelySupportedImageFormat(IImageFormat imageFormat) {

            return NativelySupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(null);

        }

        public static IImageCodec FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            return FromImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static IImageCodec FromImageFormat(IImageFormat imageFormat) {

            return GetImageCodecs(imageFormat).FirstOrDefault(codec => codec.IsSupportedImageFormat(imageFormat));

        }

        // Private members

        private static Lazy<bool> IsImageMagickAvailable { get; } = new Lazy<bool>(GetIsImageMagickAvailable);

#if NETFRAMEWORK

        private static Lazy<bool> IsWebPWrapperAvailable { get; } = new Lazy<bool>(GetIsWebPWrapperAvailable);

        private static bool GetIsWebPWrapperAvailable() {

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

        private static IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return GetImageCodecs().SelectMany(codec => codec.SupportedImageFormats)
                .Distinct(new ImageFormatComparer())
                .OrderBy(type => type);

        }
        private static IEnumerable<IImageFormat> GetNativelySupportedImageFormats() {

            return new List<string>(new[]{
                ".bmp",
                ".gif",
                ".exif",
                ".jpg",
                ".jpeg",
                ".png",
                ".tif",
                ".tiff"
            }).OrderBy(type => type)
            .Select(ext => ImageFormat.FromFileExtension(ext))
            .Distinct(new ImageFormatComparer());

        }
        private static IEnumerable<IImageCodec> GetImageCodecs(IImageFormat imageFormat) {

            List<IImageCodec> imageCodecs = new List<IImageCodec>();

            if (IsImageMagickAvailable.Value) {

                if (imageFormat is null)
                    imageCodecs.Add(new MagickImageCodec());
                else if (new MagickImageCodec().IsSupportedImageFormat(imageFormat))
                    imageCodecs.Add(new MagickImageCodec(imageFormat));

            }

#if NETFRAMEWORK

            if (imageFormat is null)
                imageCodecs.Add(new GdiImageCodec());
            else if (IsNativelySupportedImageFormat(imageFormat))
                imageCodecs.Add(new GdiImageCodec(imageFormat));

            if (IsWebPWrapperAvailable.Value)
                imageCodecs.Add(new WebPImageCodec());

#endif

            return imageCodecs;

        }

    }

}
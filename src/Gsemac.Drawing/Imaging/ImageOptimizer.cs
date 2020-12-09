using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;
using Gsemac.IO;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageOptimizer {

        // Public members

        public static IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();

        public static bool IsSupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return IsSupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(IImageFormat imageFormat) {

            return SupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

        public static IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return GetImageOptimizersInternal();

        }

        public static IImageOptimizer FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            return FromImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static IImageOptimizer FromImageFormat(IImageFormat imageFormat) {

            return GetImageOptimizers().FirstOrDefault(optimizer => optimizer.IsSupportedImageFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return GetImageOptimizers().SelectMany(optimizer => optimizer.SupportedImageFormats)
                .Distinct(new ImageFormatComparer())
                .OrderBy(type => type);

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            List<IImageOptimizer> imageOptimizers = new List<IImageOptimizer>();

#if NETFRAMEWORK

            // The NQuantImageOptimizer provides better performance for PNGs, so prioritize it over the MagickImageOptimizer.

            if (Plugins.IsNQuantAvailable.Value)
                imageOptimizers.Add(new NQuantImageOptimizer());

#endif

            if (Plugins.IsImageMagickAvailable.Value)
                imageOptimizers.Add(new MagickImageOptimizer());

            return imageOptimizers;

        }

    }

}
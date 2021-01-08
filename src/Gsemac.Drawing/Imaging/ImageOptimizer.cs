using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageOptimizer {

        // Public members

        public static IEnumerable<IFileFormat> SupportedFileFormats => GetSupportedImageFormats();

        public static bool IsSupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            return IsSupportedImageFormat(FileFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(IFileFormat imageFormat) {

            return SupportedFileFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

        public static IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return GetImageOptimizersInternal();

        }

        public static IImageOptimizer FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return FromImageFormat(FileFormat.FromFileExtension(ext));

        }
        public static IImageOptimizer FromImageFormat(IFileFormat imageFormat) {

            return GetImageOptimizers().FirstOrDefault(optimizer => optimizer.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return GetImageOptimizers().SelectMany(optimizer => optimizer.SupportedFileFormats)
                .OrderBy(type => type)
                .Distinct();

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            return ImagingPluginLoader.GetImageOptimizers();

        }

    }

}
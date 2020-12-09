using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;
using Gsemac.Drawing.Internal;
using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageOptimizer {

        // Public members

        public static IEnumerable<IImageFormat> SupportedImageFormats => supportedImageFormats.Value;

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

        private static readonly Lazy<IEnumerable<Type>> imageOptimizerTypes = new Lazy<IEnumerable<Type>>(Plugins.GetImageOptimizers);
        private static readonly Lazy<IEnumerable<IImageFormat>> supportedImageFormats = new Lazy<IEnumerable<IImageFormat>>(GetSupportedImageFormats);

        private static IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return GetImageOptimizers().SelectMany(optimizer => optimizer.SupportedImageFormats)
                .Distinct(new ImageFormatComparer())
                .OrderBy(type => type);

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            List<IImageOptimizer> imageOptimizers = new List<IImageOptimizer>();

            foreach (Type imageOptimizerType in imageOptimizerTypes.Value) {

                IImageOptimizer imageOptimizer = (IImageOptimizer)Activator.CreateInstance(imageOptimizerType);

                imageOptimizers.Add(imageOptimizer);

            }

            return imageOptimizers.OrderBy(imageOptimizer => imageOptimizer.Priority);

        }

    }

}
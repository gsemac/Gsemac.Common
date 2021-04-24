using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageOptimizerFactory :
        IImageOptimizerFactory {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => GetSupportedImageFormats();

        public static ImageOptimizerFactory Default => new ImageOptimizerFactory();

        public IImageOptimizer FromFileFormat(IFileFormat imageFormat) {

            return GetImageOptimizers().FirstOrDefault(optimizer => optimizer.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return GetImageOptimizers().SelectMany(optimizer => optimizer.SupportedFileFormats)
                .OrderBy(type => type)
                .Distinct();

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizers() {

            return GetImageOptimizersInternal();

        }
        private static IEnumerable<IImageOptimizer> GetImageOptimizersInternal() {

            return ImagingPluginLoader.GetImageOptimizers();

        }

    }

}
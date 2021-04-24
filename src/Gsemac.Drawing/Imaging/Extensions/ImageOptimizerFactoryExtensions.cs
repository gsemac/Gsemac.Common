using Gsemac.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageOptimizerFactoryExtensions {

        public static IImageOptimizer FromFileExtension(this IImageOptimizerFactory imageOptimizerFactory, string filename) {

            string ext = PathUtilities.GetFileExtension(filename);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return imageOptimizerFactory.FromFileFormat(FileFormatFactory.Default.FromFileExtension(ext));

        }

    }

}
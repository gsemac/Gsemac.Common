using Gsemac.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class HasSupportedImageFormatsExtensions {

        public static bool IsSupportedImageFormat(this IHasSupportedImageFormats obj, string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext))
                return false;

            return obj.IsSupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(this IHasSupportedImageFormats obj, IImageFormat imageFormat) {

            return obj.SupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

    }

}
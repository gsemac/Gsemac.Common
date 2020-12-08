using System.Drawing;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

#if NETFRAMEWORK

        public static bool HasIndexedPixelFormat(this Image image) {

            return image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format4bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ||
                image.PixelFormat == System.Drawing.Imaging.PixelFormat.Indexed;

        }

#endif

    }

}
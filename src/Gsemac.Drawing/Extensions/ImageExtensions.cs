#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        public static bool HasIndexedPixelFormat(this Image image) {

            return image.PixelFormat == PixelFormat.Format1bppIndexed ||
                image.PixelFormat == PixelFormat.Format4bppIndexed ||
                image.PixelFormat == PixelFormat.Format8bppIndexed ||
                image.PixelFormat == PixelFormat.Indexed;

        }

    }

}

#endif
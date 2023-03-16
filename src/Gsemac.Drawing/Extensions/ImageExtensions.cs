using Gsemac.Drawing.Imaging;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Extensions {

    public static class ImageExtensions {

        // Public members

        public static bool IsAnimated(this IImage image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return image.FrameCount > 1;

        }

        public static Image ToBitmap(this IImage image, IBitmapOptions options) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Bitmap bitmap = image.ToBitmap();

            if (options.DisposeSourceImage)
                image.Dispose();

            return bitmap;

        }

    }

}
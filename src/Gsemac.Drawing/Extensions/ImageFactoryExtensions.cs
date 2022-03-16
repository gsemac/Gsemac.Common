using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.IO;
using Gsemac.Drawing.Imaging;
using System;

#if NETFRAMEWORK
using System.Drawing;
#endif

namespace Gsemac.Drawing.Extensions {

    public static class ImageFactoryExtensions {

        // Public members

        public static IImage FromFile(this IImageFactory imageFactory, string filePath) {

            return FromFile(imageFactory, filePath, ImageDecoderOptions.Default);

        }
        public static IImage FromFile(this IImageFactory imageFactory, string filePath, IImageDecoderOptions options) {

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.Format is null) {

                options = new ImageDecoderOptions(options) {
                    Format = FileFormatFactory.Default.FromFile(filePath),
                };

            }

            using (Stream stream = File.OpenRead(filePath))
                return imageFactory.FromStream(stream, options);

        }
        public static IImage FromStream(this IImageFactory imageFactory, Stream stream) {

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            return imageFactory.FromStream(stream, ImageDecoderOptions.Default);

        }

#if NETFRAMEWORK

        public static IImage FromBitmap(this IImageFactory imageFactory, Image image) {

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return imageFactory.FromBitmap(image, BitmapToImageOptions.Default);

        }

#endif

    }

}
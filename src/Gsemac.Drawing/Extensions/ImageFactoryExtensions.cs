using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class ImageFactoryExtensions {

        // Public members

        public static IImage FromFile(this IImageFactory imageFactory, string filePath) {

            return FromFile(imageFactory, filePath, DecoderOptions.Default);

        }
        public static IImage FromFile(this IImageFactory imageFactory, string filePath, IDecoderOptions options) {

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.Format is null) {

                options = new DecoderOptions(options) {
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

            return imageFactory.FromStream(stream, DecoderOptions.Default);

        }

        public static IImage FromBitmap(this IImageFactory imageFactory, Image image) {

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return imageFactory.FromBitmap(image, null, null);

        }
        public static IImage FromBitmap(this IImageFactory imageFactory, Image image, IFileFormat format, IImageCodec codec) {

            // The "format" and "codec" arguments are allowed to be null, and will be set to defaults by GdiImage's constructor.

            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory));

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            return new GdiImage(image, format, codec);

        }

    }

}
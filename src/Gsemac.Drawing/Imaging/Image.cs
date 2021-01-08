using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public static class Image {

        public static IImage FromFile(string filePath) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw new UnsupportedFileFormatException();

            return imageCodec.Decode(filePath);

        }

#if NETFRAMEWORK

        public static IImage FromBitmap(Bitmap bitmap) {

            return FromBitmap((System.Drawing.Image)bitmap);

        }
        public static IImage FromBitmap(System.Drawing.Image bitmap) {

            return FromBitmap(bitmap, null, null);

        }
        public static IImage FromBitmap(System.Drawing.Image bitmap, IFileFormat imageFormat, IImageCodec imageCodec) {

            return new GdiImage(bitmap, imageFormat, imageCodec);

        }

#endif

    }

}
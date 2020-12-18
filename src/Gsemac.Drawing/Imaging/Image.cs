using Gsemac.Drawing.Imaging.Extensions;

namespace Gsemac.Drawing.Imaging {

    public static class Image {

        public static IImage FromFile(string filePath) {

            IImageCodec imageCodec = ImageCodec.FromFileExtension(filePath);

            if (imageCodec is null)
                throw new ImageFormatException();

            return imageCodec.Decode(filePath);

        }

#if NETFRAMEWORK

        public static IImage FromBitmap(Bitmap bitmap) {

            return FromBitmap((System.Drawing.Image)bitmap);

        }
        public static IImage FromBitmap(System.Drawing.Image bitmap) {

            return FromBitmap(bitmap, null, null);

        }
        public static IImage FromBitmap(System.Drawing.Image bitmap, IImageFormat imageFormat, IImageCodec imageCodec) {

            return new GdiImage(bitmap, imageFormat, imageCodec);

        }

#endif

    }

}
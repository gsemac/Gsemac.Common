#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public class GdiImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => image.Size;
        public IImageFormat Format => GetImageFormatFromImageFormat(originalFormat);
        public IImageCodec Codec { get; }
        public System.Drawing.Image BaseImage => image;

        public GdiImage(System.Drawing.Image image, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;
            this.Codec = codec;
            this.originalFormat = image.RawFormat;

        }
        public GdiImage(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat originalFormat, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (originalFormat is null)
                throw new ArgumentNullException(nameof(originalFormat));

            this.image = image;
            this.Codec = codec;
            this.originalFormat = originalFormat;

        }

        public Bitmap ToBitmap(bool disposeOriginal = false) {

            // The cloned bitmap will share image data with the source bitmap.
            // This allows us to avoid copying the bitmap while allowing the user to call Dispose().

            Bitmap bitmap = (Bitmap)image.Clone();

            if (disposeOriginal)
                Dispose();

            return bitmap;

        }

        public void Dispose() {

            Dispose(disposing: true);

            System.GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    image.Dispose();

                }

                disposedValue = true;

            }
        }

        // Private members

        private readonly System.Drawing.Image image;
        private readonly System.Drawing.Imaging.ImageFormat originalFormat;
        private bool disposedValue = false;

        private static IImageFormat GetImageFormatFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return Imaging.ImageFormat.FromFileExtension(".bmp");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return Imaging.ImageFormat.FromFileExtension(".gif");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                return Imaging.ImageFormat.FromFileExtension(".exif");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return Imaging.ImageFormat.FromFileExtension(".jpeg");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return Imaging.ImageFormat.FromFileExtension(".png");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return Imaging.ImageFormat.FromFileExtension(".tiff");
            else
                return null;

        }

    }

}

#endif
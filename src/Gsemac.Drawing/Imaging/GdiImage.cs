#if NETFRAMEWORK

using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    internal class GdiImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => image.Size;
        public IImageFormat Format { get; }
        public IImageCodec Codec { get; }
        internal System.Drawing.Image BaseImage => image;

        public GdiImage(System.Drawing.Image image, IImageFormat imageFormat, IImageCodec imageCodec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (imageFormat is null)
                imageFormat = GetImageFormatFromImageFormat(image.RawFormat);

            this.image = image;
            this.Format = imageFormat;
            this.Codec = imageCodec ?? (imageFormat is null ? new GdiImageCodec() : new GdiImageCodec(imageFormat));

        }
        public GdiImage(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageFormat, IImageCodec imageCodec) :
            this(image, GetImageFormatFromImageFormat(imageFormat), imageCodec) {
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
        private bool disposedValue = false;

        private static IImageFormat GetImageFormatFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return ImageFormat.FromFileExtension(".bmp");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return ImageFormat.FromFileExtension(".gif");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                return ImageFormat.FromFileExtension(".exif");
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return ImageFormat.Png;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return ImageFormat.FromFileExtension(".tiff");
            else
                return null;

        }

    }

}

#endif
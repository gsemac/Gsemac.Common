#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.IO.FileFormats;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    internal class GdiImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => image.Size;
        public IFileFormat Format { get; }
        public IImageCodec Codec { get; }
        internal Image BaseImage => image;

        public GdiImage(Image image, IFileFormat imageFormat, IImageCodec imageCodec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (imageFormat is null)
                imageFormat = GetImageFormatFromImageFormat(image.RawFormat);

            this.image = (Image)image.Clone();
            Format = imageFormat;
            Codec = imageCodec ?? (imageFormat is null ? new GdiImageCodec() : new GdiImageCodec(imageFormat));

        }
        public GdiImage(Image image, System.Drawing.Imaging.ImageFormat imageFormat, IImageCodec imageCodec) :
            this(image, GetImageFormatFromImageFormat(imageFormat), imageCodec) {
        }

        public IImage Clone() {

            return new GdiImage((Bitmap)image.Clone(), Format, Codec);

        }
        public Bitmap ToBitmap() {

            // The cloned bitmap will share image data with the source bitmap.
            // This allows us to avoid copying the bitmap while allowing the user to call Dispose() on either Bitmap without affecting the other.

            return (Bitmap)image.Clone();

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

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

        private readonly Image image;
        private bool disposedValue = false;

        private static IFileFormat GetImageFormatFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return ImageFormat.Bmp;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return ImageFormat.Gif;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return ImageFormat.Png;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else if (imageFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return ImageFormat.Ico;
            else
                return null;

        }

    }

}

#endif
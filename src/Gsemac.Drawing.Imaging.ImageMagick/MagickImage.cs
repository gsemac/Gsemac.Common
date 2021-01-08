using Gsemac.IO;
using ImageMagick;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    internal class MagickImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => new Size(Width, Height);
        public IFileFormat Format { get; }
        public IImageCodec Codec { get; }
        internal ImageMagick.MagickImage BaseImage => image;

        public MagickImage(ImageMagick.MagickImage image, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;
            Format = GetImageFormatFromMagickFormat(image.Format);
            Codec = codec;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

#if NETFRAMEWORK
        public Bitmap ToBitmap(bool disposeOriginal = false) {

            Bitmap bitmap = image.ToBitmap();

            if (disposeOriginal)
                Dispose();

            return bitmap;

        }
#endif

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

        private readonly ImageMagick.MagickImage image;
        private bool disposedValue = false;

        private static IFileFormat GetImageFormatFromMagickFormat(MagickFormat magickFormat) {

            string ext = ImageMagickUtilities.GetFileExtensionFromMagickFormat(magickFormat);

            if (string.IsNullOrEmpty(ext))
                throw new UnsupportedFileFormatException();

            return FileFormat.FromFileExtension(ext);

        }

    }

}
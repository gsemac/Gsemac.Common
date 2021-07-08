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

        public MagickImage(ImageMagick.MagickImage image, IImageCodec codec) :
            this(image, GetImageFormatFromMagickFormat(image.Format), codec) {
        }
        public MagickImage(ImageMagick.MagickImage image, IFileFormat imageFormat, IImageCodec codec) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (codec is null)
                throw new ArgumentNullException(nameof(codec));

            this.image = image;
            this.Format = imageFormat;
            this.Codec = codec;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

#if NETFRAMEWORK
        public Bitmap ToBitmap() {

            return image.ToBitmap();

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
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            return FileFormatFactory.Default.FromFileExtension(ext);

        }

    }

}
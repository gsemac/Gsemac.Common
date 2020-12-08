using Gsemac.Drawing.Imaging;
using ImageMagick;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing {

    public class MagickImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => new Size(Width, Height);

        public MagickImage(ImageMagick.MagickImage image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;

        }

        public void Save(Stream stream) {

            image.Write(stream);

        }
        public void Save(Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            image.Format = GetMagickFormatForFileExtension(imageFormat.FileExtension);
            image.Quality = encoderOptions.Quality;

            image.Write(stream);

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

        private static MagickFormat GetMagickFormatForFileExtension(string fileExtension) {

            switch (fileExtension.ToLowerInvariant()) {

                case ".avif":
                    return MagickFormat.Avif;

                case ".bmp":
                    return MagickFormat.Bmp;

                case ".gif":
                    return MagickFormat.Gif;

                case ".jpg":
                    return MagickFormat.Jpg;

                case ".jpeg":
                    return MagickFormat.Jpeg;

                case ".png":
                    return MagickFormat.Png;

                case ".tif":
                    return MagickFormat.Tif;

                case ".tiff":
                    return MagickFormat.Tiff;

                case ".webp":
                    return MagickFormat.WebP;

                default:
                    throw new ArgumentException("The file extension was not recognized.");

            }

        }

    }

}
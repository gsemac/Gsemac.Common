#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing {

    public class GdiImage :
        IImage {

        // Public members

        public int Width => image.Width;
        public int Height => image.Height;
        public Size Size => image.Size;
        public IImageFormat ImageFormat => GetImageFormatFromImageFormat(originalFormat);

        public GdiImage(Image image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;
            this.originalFormat = image.RawFormat;

        }
        public GdiImage(Image image, System.Drawing.Imaging.ImageFormat originalFormat) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (originalFormat is null)
                throw new ArgumentNullException(nameof(originalFormat));

            this.image = image;
            this.originalFormat = originalFormat;

        }

        public void Save(Stream stream) {

            ImageCodecInfo encoder = GetEncoderFromImageFormat(originalFormat);

            if (encoder is null)
                encoder = GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat.Png);

            image.Save(stream, encoder, null);

        }
        public void Save(Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, encoderOptions.Quality)) {

                encoderParameters.Param[0] = qualityParameter;

                System.Drawing.Imaging.ImageFormat format = imageFormat is null ? image.RawFormat : GetImageFormatFromFileExtension(imageFormat.FileExtension);
                ImageCodecInfo encoder = GetEncoderFromImageFormat(format);

                if (encoder is null)
                    throw new ArgumentException(nameof(imageFormat));

                image.Save(stream, encoder, encoderParameters);

            }

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

        private readonly Image image;
        private readonly System.Drawing.Imaging.ImageFormat originalFormat;
        private bool disposedValue = false;

        private static System.Drawing.Imaging.ImageFormat GetImageFormatFromFileExtension(string fileExtension) {

            switch (fileExtension.ToLowerInvariant()) {

                case ".bmp":
                    return System.Drawing.Imaging.ImageFormat.Bmp;

                case ".gif":
                    return System.Drawing.Imaging.ImageFormat.Gif;

                case ".exif":
                    return System.Drawing.Imaging.ImageFormat.Exif;

                case ".jpg":
                case ".jpeg":
                    return System.Drawing.Imaging.ImageFormat.Jpeg;

                case ".png":
                    return System.Drawing.Imaging.ImageFormat.Png;

                case ".tif":
                case ".tiff":
                    return System.Drawing.Imaging.ImageFormat.Tiff;

                default:
                    throw new ArgumentException("The file extension was not recognized.");

            }

        }
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
        private ImageCodecInfo GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            ImageCodecInfo encoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return encoder;

        }

    }

}

#endif
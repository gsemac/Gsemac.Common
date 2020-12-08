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

        public GdiImage(Image image) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            this.image = image;

        }

        public Bitmap ToBitmap(bool disposeOriginal = false) {

            // The cloned bitmap will share image data with the source bitmap.
            // This allows us to avoid copying the bitmap while allowing the user to call Dispose().

            Bitmap bitmap = (Bitmap)image.Clone();

            if (disposeOriginal)
                image.Dispose();

            return bitmap;

        }

        public void Save(Stream stream) {

            image.Save(stream, image.RawFormat);

        }
        public void Save(Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions) {

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, encoderOptions.Quality)) {

                encoderParameters.Param[0] = qualityParameter;

                System.Drawing.Imaging.ImageFormat format = imageFormat is null ? image.RawFormat : GetImageFormatForFileExtension(imageFormat.FileExtension);
                ImageCodecInfo encoder = GetEncoderForImageFormat(format);

                if (encoder is null)
                    throw new ArgumentException(nameof(imageFormat));

                image.Save(stream, encoder, encoderParameters);

            }

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
        private bool disposedValue = false;

        private static System.Drawing.Imaging.ImageFormat GetImageFormatForFileExtension(string fileExtension) {

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
        private ImageCodecInfo GetEncoderForImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            ImageCodecInfo decoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return decoder;

        }

    }

}

#endif
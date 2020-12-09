#if NETFRAMEWORK

using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Imaging.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class GdiImageCodec :
        IImageCodec {

        // Public members

        public IEnumerable<IImageFormat> SupportedImageFormats => ImageCodec.NativelySupportedImageFormats;
        public int Priority => 0;

        public GdiImageCodec() {
        }
        public GdiImageCodec(IImageFormat imageFormat) {

            if (!this.IsSupportedImageFormat(imageFormat.FileExtension))
                throw ImageExceptions.UnsupportedImageFormat;

            this.imageFormat = imageFormat;

        }

        public Image Decode(Stream stream) {

            // Image requires that the stream be kept open, because it is read lazily.
            // By creating a new Bitmap from the image, we force it to read the stream immediately.

            using (Image imageFromStream = Image.FromStream(stream))
                return new Bitmap(imageFromStream);

        }
        IImage IImageDecoder.Decode(Stream stream) {

            // When we create a new Bitmap from the Image, we lose information about its original format (it just becomes a memory Bitmap).
            // This GdiImage constructor allows us to preserve the original format information.

            using (Image imageFromStream = Image.FromStream(stream))
                return new GdiImage(new Bitmap(imageFromStream), imageFromStream.RawFormat, this);

        }
        public void Encode(Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            Encode(new GdiImage(image, this), stream, encoderOptions);

        }
        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is GdiImage gdiImage) {

                // If the image is aleady a GdiImage, we can save it directly.

                Save(gdiImage.BaseImage, stream, encoderOptions);

            }
            else {

                // If the image is not a GdiImage, convert it to a bitmap and load it.

                using (Bitmap intermediateBitmap = image.ToBitmap())
                using (gdiImage = new GdiImage(intermediateBitmap, this))
                    Save(gdiImage.BaseImage, stream, encoderOptions);

            }

        }

        // Private members

        private readonly IImageFormat imageFormat;

        private void Save(Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, encoderOptions.Quality)) {

                encoderParameters.Param[0] = qualityParameter;

                System.Drawing.Imaging.ImageFormat format = imageFormat is null ? image.RawFormat : GetImageFormatFromFileExtension(imageFormat.FileExtension);
                ImageCodecInfo encoder = GetEncoderFromImageFormat(format);

                if (encoder is null)
                    encoder = GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat.Png);

                image.Save(stream, encoder, encoderParameters);

            }

        }

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
        private ImageCodecInfo GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            ImageCodecInfo encoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return encoder;

        }

    }

}

#endif
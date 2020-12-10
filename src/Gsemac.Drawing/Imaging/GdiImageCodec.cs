#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.Drawing.Internal;
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
                throw new ImageFormatException();

            this.imageFormat = imageFormat;

        }

        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is GdiImage gdiImage) {

                // If the image is aleady a GdiImage, we can save it directly.

                EncodeBitmap(gdiImage.BaseImage, stream, encoderOptions);

            }
            else {

                // If the image is not a GdiImage, convert it to a bitmap and load it.

                using (Bitmap intermediateBitmap = image.ToBitmap())
                using (gdiImage = new GdiImage(intermediateBitmap, intermediateBitmap.RawFormat, this))
                    EncodeBitmap(gdiImage.BaseImage, stream, encoderOptions);

            }

        }
        public IImage Decode(Stream stream) {

            // When we create a new Bitmap from the Image, we lose information about its original format (it just becomes a memory Bitmap).
            // This GdiImage constructor allows us to preserve the original format information.

            using (System.Drawing.Image imageFromStream = System.Drawing.Image.FromStream(stream))
                return new GdiImage(new Bitmap(imageFromStream), imageFromStream.RawFormat, this);

        }

        // Private members

        private readonly IImageFormat imageFormat;

        private void EncodeBitmap(System.Drawing.Image image, Stream stream, IImageEncoderOptions encoderOptions) {

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
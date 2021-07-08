#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class GdiImageCodec :
        PluginBase,
        IImageCodec {

        // Public members

        public GdiImageCodec() {
        }
        public GdiImageCodec(IFileFormat imageFormat) {

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (!this.IsSupportedFileFormat(imageFormat))
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            this.imageFormat = imageFormat;

        }

        public void Encode(IImage image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

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

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            // When we create a new Bitmap from the Image, we lose information about its original format (it just becomes a memory Bitmap).
            // This GdiImage constructor allows us to preserve the original format information.

            // Note that despite the fact the ICO image format is supported, decoding may still fail if the icon is of the newer format added in Windows Vista (which contains an appended PNG).
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/a4125122-63de-439a-bc33-cee2c65ec0ae/imagefromstream-and-imagefromfile-and-different-icon-files?forum=winforms

            using (Image imageFromStream = Image.FromStream(stream))
                return new GdiImage(new Bitmap(imageFromStream), imageFromStream.RawFormat, this);

        }

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetNativelySupportedImageFormats();

        }

        public static bool IsSupportedFileFormat(string filePath) {

            return new GdiImageCodec().IsSupportedFileFormat(filePath);

        }
        public static bool IsSupportedFileFormat(IFileFormat fileFormat) {

            return new GdiImageCodec().IsSupportedFileFormat(fileFormat);

        }

        // Private members

        private readonly IFileFormat imageFormat;

        private void EncodeBitmap(Image image, Stream stream, IImageEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, encoderOptions.Quality)) {

                encoderParameters.Param[0] = qualityParameter;

                System.Drawing.Imaging.ImageFormat format = imageFormat is null ? image.RawFormat : GetImageFormatFromFileExtension(imageFormat.Extensions.FirstOrDefault());
                ImageCodecInfo encoder = GetEncoderFromImageFormat(format);

                if (encoder is null)
                    encoder = GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat.Png);

                image.Save(stream, encoder, encoderParameters);

            }

        }

        private static IEnumerable<IFileFormat> GetNativelySupportedImageFormats() {

            return new List<string>(new[]{
                ".bmp",
                //".exif",
                ".gif",
                ".ico",
                ".jpeg",
                ".jpg",
                ".png",
                ".tif",
                ".tiff",
                ".wmf",
            }).OrderBy(type => type)
            .Select(ext => FileFormatFactory.Default.FromFileExtension(ext))
            .Distinct();

        }
        private static System.Drawing.Imaging.ImageFormat GetImageFormatFromFileExtension(string fileExtension) {

            if (fileExtension is null)
                throw new ArgumentNullException(nameof(fileExtension));

            switch (fileExtension.ToLowerInvariant()) {

                case ".bmp":
                    return System.Drawing.Imaging.ImageFormat.Bmp;

                case ".gif":
                    return System.Drawing.Imaging.ImageFormat.Gif;

                case ".exif":
                    return System.Drawing.Imaging.ImageFormat.Exif;

                case ".ico":
                    return System.Drawing.Imaging.ImageFormat.Icon;

                case ".jpg":
                case ".jpeg":
                    return System.Drawing.Imaging.ImageFormat.Jpeg;

                case ".png":
                    return System.Drawing.Imaging.ImageFormat.Png;

                case ".tif":
                case ".tiff":
                    return System.Drawing.Imaging.ImageFormat.Tiff;

                case ".wmf":
                    return System.Drawing.Imaging.ImageFormat.Wmf;

                default:
                    throw new ArgumentException("The file extension was not recognized.");

            }

        }
        private ImageCodecInfo GetEncoderFromImageFormat(System.Drawing.Imaging.ImageFormat imageFormat) {

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            ImageCodecInfo encoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return encoder;

        }

    }

}

#endif
using Gsemac.IO;
using Gsemac.IO.Extensions;
using Gsemac.Reflection.Plugins;
using Gsemac.Win32;
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

        public void Encode(IImage image, Stream stream, IEncoderOptions encoderOptions) {

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
                using (gdiImage = new GdiImage(intermediateBitmap, this))
                    EncodeBitmap(gdiImage.BaseImage, stream, encoderOptions);

            }

        }
        public IImage Decode(Stream stream, IDecoderOptions options) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            // When we create a new Bitmap from the Image, we lose information about its original format (it just becomes a memory Bitmap).
            // This GdiImage constructor allows us to preserve the original format information.

            // After creating an image from the source stream, we construct another bitmap from it.
            // The stream is normally lazy-loaded when the image data is requested, and this causes it to be loaded immediately so the stream doesn't have to stay open.
            // https://stackoverflow.com/a/13935966/5383169 (Anlo)

            // Note that despite the fact the ICO image format is supported, decoding may still fail if the icon is of the newer format added in Windows Vista (which contains an appended PNG).
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/a4125122-63de-439a-bc33-cee2c65ec0ae/imagefromstream-and-imagefromfile-and-different-icon-files?forum=winforms

            using (Image originalImage = Image.FromStream(stream))
            using (Image nonDeferredImage = new Bitmap(originalImage))
                return new GdiImage(originalImage, nonDeferredImage, this);

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

        private void EncodeBitmap(Image image, Stream stream, IEncoderOptions encoderOptions) {

            if (image is null)
                throw new ArgumentNullException(nameof(image));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            ImageFormat format = imageFormat is null ? image.RawFormat : GetImageFormatFromFileExtension(imageFormat.Extensions.FirstOrDefault());

            // The Save method cannot encode WMF images.
            // https://stackoverflow.com/questions/5270763/convert-an-image-into-wmf-with-net

            if (format.Equals(ImageFormat.Wmf)) {

                EncodeBitmapToWmf(image, stream);

            }
            else {

                using (EncoderParameters encoderParameters = new EncoderParameters(1))
                using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, encoderOptions.Quality)) {

                    encoderParameters.Param[0] = qualityParameter;

                    ImageCodecInfo encoder = GetEncoderFromImageFormat(format);

                    if (encoder is null)
                        encoder = GetEncoderFromImageFormat(ImageFormat.Png);

                    image.Save(stream, encoder, encoderParameters);

                }

            }

        }
        private void EncodeBitmapToWmf(Image image, Stream stream) {

            // The following approach was adapted from the solution given here: https://stackoverflow.com/a/27284866/5383169 (ILIA BROUDNO)

            // Unfortunately, it seems that the image is scaled to match the dimensions of the display device.
            // There is some discussion of the issue here: https://stackoverflow.com/questions/53530519/what-governs-dc-scaling
            // How can I fix this to maintain the original image dimensions?

            Metafile metafile = null;

            try {

                using (Graphics graphics = Graphics.FromImage(image)) {

                    IntPtr hdc = graphics.GetHdc();

                    metafile = new Metafile(hdc, new Rectangle(0, 0, image.Width, image.Height), MetafileFrameUnit.Pixel, EmfType.EmfOnly);

                    graphics.ReleaseHdc(hdc);

                }

                using (Graphics graphics = Graphics.FromImage(metafile))
                    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                IntPtr hEmf = metafile.GetHenhmetafile();

                uint bufferSize = GdiPlus.GdipEmfToWmfBits(hEmf, 0, null, Gdi32.MM_ANISOTROPIC, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
                byte[] buffer = new byte[bufferSize];

                GdiPlus.GdipEmfToWmfBits(hEmf, bufferSize, buffer, Gdi32.MM_ANISOTROPIC, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                Gdi32.DeleteEnhMetaFile(hEmf);

                stream.Write(buffer, 0, (int)bufferSize);

            }
            finally {

                if (metafile is object)
                    metafile.Dispose();

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
        private static ImageFormat GetImageFormatFromFileExtension(string fileExtension) {

            if (fileExtension is null)
                throw new ArgumentNullException(nameof(fileExtension));

            switch (fileExtension.ToLowerInvariant()) {

                case ".bmp":
                    return ImageFormat.Bmp;

                case ".gif":
                    return ImageFormat.Gif;

                case ".exif":
                    return ImageFormat.Exif;

                case ".ico":
                    return ImageFormat.Icon;

                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;

                case ".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new ArgumentException("The file extension was not recognized.");

            }

        }
        private ImageCodecInfo GetEncoderFromImageFormat(ImageFormat imageFormat) {

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            ImageCodecInfo encoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == imageFormat.Guid)
                .FirstOrDefault();

            return encoder;

        }

    }

}
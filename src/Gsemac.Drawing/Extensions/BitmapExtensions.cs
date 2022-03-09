#if NETFRAMEWORK

using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class BitmapExtensions {

        // Public members

        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat, IImageEncoder encoder, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            using (FileStream fs = File.OpenWrite(filePath))
                Save(bitmap, fs, imageFormat, encoder, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            if (encoderOptions is null)
                throw new ArgumentNullException(nameof(encoderOptions));

            IImageCodec imageCodec = ImageCodecFactory.Default.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, filePath, imageFormat, imageCodec, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath, IFileFormat imageFormat) {

            Save(bitmap, filePath, imageFormat, ImageEncoderOptions.Default);

        }
        public static void Save(this Image bitmap, string filePath, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (filePath is null)
                throw new ArgumentNullException(nameof(filePath));

            IFileFormat imageFormat = FileFormatFactory.Default.FromFileExtension(filePath);

            if (imageFormat is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, filePath, imageFormat, encoderOptions);

        }
        public static void Save(this Image bitmap, string filePath) {

            Save(bitmap, filePath, ImageEncoderOptions.Default);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat, IImageEncoder encoder, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            using (IImage image = ImageFactory.Default.FromBitmap(bitmap))
                encoder.Encode(image, stream, encoderOptions);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat, IImageEncoderOptions encoderOptions) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (imageFormat is null)
                throw new ArgumentNullException(nameof(imageFormat));

            IImageCodec imageCodec = ImageCodecFactory.Default.FromFileFormat(imageFormat);

            if (imageCodec is null)
                throw new FileFormatException(IO.Properties.ExceptionMessages.UnsupportedFileFormat);

            Save(bitmap, stream, imageFormat, imageCodec, encoderOptions);

        }
        public static void Save(this Image bitmap, Stream stream, IFileFormat imageFormat) {

            Save(bitmap, stream, imageFormat, ImageEncoderOptions.Default);

        }

        public static Image Resize(this Image bitmap, IImageResizingOptions options) {

            if (bitmap is null)
                throw new ArgumentNullException(nameof(bitmap));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (!options.Width.HasValue && !options.Height.HasValue && !options.HorizontalScale.HasValue && !options.VerticalScale.HasValue)
                return (Image)bitmap.Clone();

            if (options.SizingMode == ImageSizingMode.ResizeIfLarger) {

                if ((!options.Width.HasValue || bitmap.Width <= options.Width.Value) && (!options.Height.HasValue || bitmap.Height <= options.Height.Value))
                    return (Image)bitmap.Clone();

            }

            if (options.SizingMode == ImageSizingMode.ResizeIfSmaller) {

                if ((!options.Width.HasValue || bitmap.Width >= options.Width.Value) && (!options.Height.HasValue || bitmap.Height >= options.Height.Value))
                    return (Image)bitmap.Clone();

            }

            int? newWidth = options.Width;
            int? newHeight = options.Height;

            if (!newWidth.HasValue && options.HorizontalScale.HasValue)
                newWidth = (int)(bitmap.Width * options.HorizontalScale.Value);

            if (!newHeight.HasValue && options.VerticalScale.HasValue)
                newHeight = (int)(bitmap.Height * options.VerticalScale.Value);

            // If the image hasn't been resized at all, just return the source image.

            if (!newWidth.HasValue && !newHeight.HasValue)
                return (Image)bitmap.Clone();

            return ResizeInternal(bitmap, width: newWidth, height: newHeight);

        }
        public static Image Resize(this Image bitmap, int? width = null, int? height = null) {

            return Resize(bitmap, new ImageResizingOptions() {
                Width = width,
                Height = height,
            });

        }
        public static Image Scale(this Image bitmap, float? horizontalScale = null, float? verticalScale = null) {

            return Resize(bitmap, new ImageResizingOptions() {
                HorizontalScale = horizontalScale,
                VerticalScale = verticalScale,
            });

        }

        // Private methods

        private static Image ResizeInternal(Image image, int? width = null, int? height = null) {

            if ((width ?? 0) <= 0)
                width = null;

            if ((height ?? 0) <= 0)
                height = null;

            int newWidth;
            int newHeight;

            if (width.HasValue && height.HasValue) {

                newWidth = width.Value;
                newHeight = height.Value;

            }
            else if (width.HasValue) {

                float scaleFactor = (float)width.Value / image.Width;

                newWidth = width.Value;
                newHeight = (int)(image.Height * scaleFactor);

            }
            else if (height.HasValue) {

                float scaleFactor = (float)height.Value / image.Height;

                newWidth = (int)(image.Width * scaleFactor);
                newHeight = height.Value;

            }
            else {

                // If no dimensions have been specified, simply return the original image.

                return image;

            }

            Bitmap resultImage = new Bitmap(image, new Size(newWidth, newHeight));

            return resultImage;

        }

    }

}

#endif
#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Gsemac.Drawing.Imaging {

    public static class ImageUtilities {

        // Public members

        public static Image OpenImage(string filePath, bool openWithoutLocking = false) {

            if (openWithoutLocking) {

                // This technique allows us to open the image without locking it, allowing the original image to be overwritten.

                using (Image image = OpenImageInternal(filePath))
                    return new Bitmap(image);

            }
            else {

                return OpenImageInternal(filePath);

            }

        }
        public static void SaveImage(Image image, string filePath, IImageEncoderOptions options = null) {

            SaveImageInternal(image, filePath, options ?? new ImageEncoderOptions());

        }
        public static Image ResizeImage(Image image, int? width = null, int? height = null, bool disposeOriginal = false) {

            int newWidth = image.Width;
            int newHeight = image.Height;

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

            Bitmap resultImage = new Bitmap(image, new Size(newWidth, newHeight));

            if (disposeOriginal)
                image.Dispose();

            return resultImage;

        }
        public static Image ConvertImageToNonIndexedPixelFormat(Image image, bool disposeOriginal = false) {

            // We can't create a graphics object from an image with an indexed pixel format, so we need to create a new bitmap.

            if (!image.HasIndexedPixelFormat())
                return image;

            Bitmap resultImage = new Bitmap(image);

            if (disposeOriginal)
                image.Dispose();

            return resultImage;

        }

        // Private members

        private static ImageFormat GetImageFormatForFileExtension(string fileExtension) {

            switch (fileExtension.ToLowerInvariant()) {

                case ".bmp":
                    return ImageFormat.Bmp;

                case ".gif":
                    return ImageFormat.Gif;

                case ".exif":
                    return ImageFormat.Exif;

                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".tif":
                case ".tiff":
                    return ImageFormat.Tiff;

                default:
                    throw new ArgumentException("Unrecognized file extension.");

            }

        }

        private static Image OpenImageInternal(string filePath) {

            IImageReader imageReader = ImageReader.Create(filePath);

            if (imageReader is null)
                throw new FileFormatException("The image format is not supported.");

            return imageReader.ReadImage(filePath);

        }
        private static void SaveImageInternal(Image image, string filePath, IImageEncoderOptions options) {

            IImageReader imageReader = ImageReader.Create(filePath);

            if (imageReader is null)
                throw new FileFormatException("The image format is not supported.");

            string ext = PathUtilities.GetFileExtension(filePath);

            if (imageReader is NativeImageReader nativeImageReader)
                nativeImageReader.SaveImage(image, filePath, GetImageFormatForFileExtension(ext), options);
            else
                imageReader.SaveImage(image, filePath, options);

        }

    }

}

#endif
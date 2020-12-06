#if NETFRAMEWORK

using Gsemac.Drawing.Extensions;
using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageUtilities {

        // Public members

        public static IEnumerable<string> SupportedFileTypes => GetSupportedFileTypes();
        public static IEnumerable<string> NativelySupportedFileTypes => GetNativelySupportedFileTypes();

        public static bool IsSupportedFileType(string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return GetSupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }
        public static bool IsNativelySupportedFileType(string filename) {

            string ext = PathUtilities.GetFileExtension(filename).ToLowerInvariant();

            return GetNativelySupportedFileTypes().Any(supportedExt => supportedExt.Equals(ext, StringComparison.OrdinalIgnoreCase));

        }

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

        private static Lazy<bool> IsWebPSupportAvailable { get; } = new Lazy<bool>(GetIsWebPSupportAvailable);

        private static bool GetIsWebPSupportAvailable() {

            AnyCpuFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver();

            // Check for the presence of the "WebPWrapper.WebP" class (in case something like ilmerge was used and the assembly is not present on disk).

            bool webPWrapperExists = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType("WebPWrapper.WebP") != null)
                .FirstOrDefault();

            // Check for WebPWrapper on disk.

            if (!webPWrapperExists)
                webPWrapperExists = assemblyResolver.AssemblyExists("WebPWrapper");

            // Check for libwebp on disk.

            bool libWebPExists = assemblyResolver.AssemblyExists(Environment.Is64BitProcess ? "libwebp_x64" : "libwebp_x86");

            return webPWrapperExists && libWebPExists;

        }

        private static IEnumerable<string> GetSupportedFileTypes() {

            return GetImageReaders().SelectMany(reader => reader.SupportedFileTypes);

        }
        private static IEnumerable<string> GetNativelySupportedFileTypes() {

            List<string> extensions = new List<string>(new[]{
                ".bmp",
                ".gif",
                ".exif",
                ".jpg",
                ".jpeg",
                ".png",
                ".tif",
                ".tiff"
            });

            return extensions;

        }

        private static IEnumerable<IImageReader> GetImageReaders() {

            List<IImageReader> imageReaders = new List<IImageReader> {
                new NativeImageReader()
            };

            if (IsWebPSupportAvailable.Value)
                imageReaders.Add(new WebPImageReader());

            return imageReaders;

        }
        private static IImageReader GetImageReader(string filePath) {

            return GetImageReaders().FirstOrDefault(reader => reader.IsSupportedFileType(filePath));

        }
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

            IImageReader imageReader = GetImageReader(filePath);

            if (imageReader is null)
                throw new FileFormatException("The image format is not supported.");

            return imageReader.ReadImage(filePath);

        }
        private static void SaveImageInternal(Image image, string filePath, IImageEncoderOptions options) {

            IImageReader imageReader = GetImageReader(filePath);

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
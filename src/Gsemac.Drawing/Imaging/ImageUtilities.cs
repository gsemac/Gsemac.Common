#if NETFRAMEWORK

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
        public static Image ResizeImage(Image sourceImage, int? width = null, int? height = null) {

            int newWidth = sourceImage.Width;
            int newHeight = sourceImage.Height;

            if (width.HasValue && height.HasValue) {

                newWidth = width.Value;
                newHeight = height.Value;

            }
            else if (width.HasValue) {

                float scaleFactor = (float)width.Value / sourceImage.Width;

                newWidth = width.Value;
                newHeight = (int)(sourceImage.Height * scaleFactor);

            }
            else if (height.HasValue) {

                float scaleFactor = (float)height.Value / sourceImage.Height;

                newWidth = (int)(sourceImage.Width * scaleFactor);
                newHeight = height.Value;

            }

            return new Bitmap(sourceImage, new Size(newWidth, newHeight));

        }

        // Private members

        private static bool? isWebpSupportAvailable;

        private static bool IsWebpSupportAvailable() {

            if (!isWebpSupportAvailable.HasValue) {

                // #todo Not this?

                AnyCpuFileSystemAssemblyResolver assemblyResolver = new AnyCpuFileSystemAssemblyResolver();

                bool webPWrapperExists = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(assembly => assembly.GetType("WebPWrapper.WebP") != null)
                    .FirstOrDefault();

                if (!webPWrapperExists)
                    webPWrapperExists = assemblyResolver.AssemblyExists("WebPWrapper");

                bool libWebPExists = assemblyResolver.AssemblyExists(Environment.Is64BitProcess ? "libwebp_x64" : "libwebp_x86");

                isWebpSupportAvailable = webPWrapperExists && libWebPExists;

            }

            return isWebpSupportAvailable.Value;

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

            if (IsWebpSupportAvailable())
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
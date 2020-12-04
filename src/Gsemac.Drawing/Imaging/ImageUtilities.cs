#if NETFRAMEWORK

using Gsemac.IO;
using Gsemac.Reflection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageUtilities {

        // Public members

        public static IEnumerable<string> GetSupportedFileExtensions() {

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

            if (IsWebpSupportAvailable())
                extensions.Add(".webp");

            return extensions;

        }
        public static bool IsFileExtensionSupported(string filename) {

            string ext = Path.GetExtension(filename).ToLowerInvariant();

            return GetSupportedFileExtensions().Any(supportedExt => supportedExt.Equals(ext));

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
        private static ImageCodecInfo GetEncoderForFileExtension(string fileExtension) {

            ImageFormat format = GetImageFormatForFileExtension(fileExtension);

            ImageCodecInfo decoder = ImageCodecInfo.GetImageDecoders()
                .Where(codec => codec.FormatID == format.Guid)
                .FirstOrDefault();

            return decoder;

        }

        private static Image OpenImageInternal(string filePath) {

            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            if (!IsFileExtensionSupported(ext))
                throw new FileFormatException("The image format is not supported.");

            if (ext.Equals(".webp", StringComparison.OrdinalIgnoreCase)) {

                return OpenWebpImage(filePath);

            }
            else {

                return new Bitmap(filePath);

            }

        }
        private static void SaveImageInternal(Image image, string filePath, IImageEncoderOptions options) {

            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            if (!IsFileExtensionSupported(ext))
                throw new FileFormatException("The image format is not supported.");

            if (ext.Equals(".webp", StringComparison.OrdinalIgnoreCase)) {

                SaveWebpImage(image, filePath, options);

            }
            else {

                using (EncoderParameters encoderParameters = new EncoderParameters(1))
                using (EncoderParameter qualityParameter = new EncoderParameter(Encoder.Quality, options.Quality)) {

                    encoderParameters.Param[0] = qualityParameter;

                    ImageCodecInfo encoder = GetEncoderForFileExtension(ext);

                    image.Save(filePath, encoder, encoderParameters);

                }

                image.Save(filePath);

            }

        }

        private static Image OpenWebpImage(string filePath) {

            // This is in a separate method so accessing WebPWrapper doesn't throw an exception when the DLL doen't exist.

            using (WebPWrapper.WebP decoder = new WebPWrapper.WebP())
                return decoder.Load(filePath);

        }
        private static void SaveWebpImage(Image image, string filePath, IImageEncoderOptions options) {

            // This is in a separate method so accessing WebPWrapper doesn't throw an exception when the DLL doen't exist.

            using (WebPWrapper.WebP encoder = new WebPWrapper.WebP())
                encoder.Save(image as Bitmap, filePath, options.Quality);

        }

    }

}

#endif
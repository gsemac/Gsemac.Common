using Gsemac.Drawing.Imaging.Extensions;
using Gsemac.IO;
using Gsemac.Reflection.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public static class ImageCodec {

        // Public members

        public static IEnumerable<IImageFormat> SupportedImageFormats => GetSupportedImageFormats();
        public static IEnumerable<IImageFormat> NativelySupportedImageFormats => GetNativelySupportedImageFormats();

        public static bool IsSupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return IsSupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsSupportedImageFormat(IImageFormat imageFormat) {

            return SupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }
        public static bool IsNativelySupportedImageFormat(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath).ToLowerInvariant();

            return IsNativelySupportedImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static bool IsNativelySupportedImageFormat(IImageFormat imageFormat) {

            return NativelySupportedImageFormats.Any(supportedImageFormat => supportedImageFormat.Equals(imageFormat));

        }

        public static IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(null);

        }

        public static IImageCodec FromFileExtension(string filePath) {

            string ext = PathUtilities.GetFileExtension(filePath);

            return FromImageFormat(ImageFormat.FromFileExtension(ext));

        }
        public static IImageCodec FromImageFormat(IImageFormat imageFormat) {

            return GetImageCodecs(imageFormat).FirstOrDefault(codec => codec.IsSupportedImageFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IImageFormat> GetSupportedImageFormats() {

            return GetImageCodecs().SelectMany(codec => codec.SupportedImageFormats)
                .Distinct(new ImageFormatComparer())
                .OrderBy(type => type);

        }
        private static IEnumerable<IImageFormat> GetNativelySupportedImageFormats() {

            return new List<string>(new[]{
                ".bmp",
                ".gif",
                ".exif",
                ".jpg",
                ".jpeg",
                ".png",
                ".tif",
                ".tiff"
            }).OrderBy(type => type)
            .Select(ext => ImageFormat.FromFileExtension(ext))
            .Distinct(new ImageFormatComparer());

        }
        private static IEnumerable<IImageCodec> GetImageCodecs(IImageFormat imageFormat) {

            List<IImageCodec> imageCodecs = new List<IImageCodec>();

            foreach (IImageCodec imageCodec in ImagingPluginLoader.GetImageCodecs()) {

                IImageCodec nextImageCodec = imageCodec;
                Type nextImageCodecType = imageCodec.GetType();

                if (!(nextImageCodec is null) && !(imageFormat is null) && nextImageCodec.IsSupportedImageFormat(imageFormat) && nextImageCodecType.GetConstructor(new[] { typeof(IImageFormat) }) != null)
                    nextImageCodec = (IImageCodec)Activator.CreateInstance(nextImageCodecType, new object[] { imageFormat });

                imageCodecs.Add(nextImageCodec);

            }

            return imageCodecs;

        }

    }

}
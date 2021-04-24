using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageCodecFactory :
        IImageCodecFactory {

        // Public members

        public IEnumerable<IFileFormat> SupportedFileFormats => GetSupportedImageFormats();
        public IEnumerable<IFileFormat> NativelySupportedFileFormats => GetNativelySupportedImageFormats();

        public static ImageCodecFactory Default => new ImageCodecFactory();

        public IImageCodec FromFileFormat(IFileFormat imageFormat) {

            return GetImageCodecs(imageFormat).FirstOrDefault(codec => codec.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(null);

        }
        private static IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return GetImageCodecs().SelectMany(codec => codec.SupportedFileFormats)
                .Distinct()
                .OrderBy(type => type);

        }
        private static IEnumerable<IFileFormat> GetNativelySupportedImageFormats() {

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
            .Select(ext => FileFormatFactory.Default.FromFileExtension(ext))
            .Distinct();

        }
        private static IEnumerable<IImageCodec> GetImageCodecs(IFileFormat imageFormat) {

            List<IImageCodec> imageCodecs = new List<IImageCodec>();

            foreach (IImageCodec imageCodec in ImagingPluginLoader.GetImageCodecs()) {

                IImageCodec nextImageCodec = imageCodec;
                Type nextImageCodecType = imageCodec.GetType();

                if (!(nextImageCodec is null) && !(imageFormat is null) && nextImageCodec.IsSupportedFileFormat(imageFormat) && nextImageCodecType.GetConstructor(new[] { typeof(IFileFormat) }) != null)
                    nextImageCodec = (IImageCodec)Activator.CreateInstance(nextImageCodecType, new object[] { imageFormat });

                imageCodecs.Add(nextImageCodec);

            }

            return imageCodecs;

        }

    }

}
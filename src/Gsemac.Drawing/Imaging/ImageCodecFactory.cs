using Gsemac.IO;
using Gsemac.IO.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.Drawing.Imaging {

    public class ImageCodecFactory :
        IImageCodecFactory {

        // Public members

        public static ImageCodecFactory Default => new ImageCodecFactory();

        public IEnumerable<IFileFormat> GetSupportedFileFormats() {

            return GetSupportedImageFormats();

        }

        public IImageCodec FromFileFormat(IFileFormat imageFormat) {

            return GetImageCodecs(imageFormat).FirstOrDefault(codec => codec.IsSupportedFileFormat(imageFormat));

        }

        // Private members

        private static IEnumerable<IImageCodec> GetImageCodecs() {

            return GetImageCodecs(null);

        }
        private static IEnumerable<IFileFormat> GetSupportedImageFormats() {

            return GetImageCodecs().SelectMany(codec => codec.GetSupportedFileFormats())
                .Distinct()
                .OrderBy(type => type);

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
using Gsemac.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecFactoryExtensions {

        public static IImageCodec FromFileExtension(this IImageCodecFactory imageCodecFactory, string filename) {

            string ext = PathUtilities.GetFileExtension(filename);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return imageCodecFactory.FromFileFormat(FileFormatFactory.Default.FromFileExtension(ext));

        }

    }

}
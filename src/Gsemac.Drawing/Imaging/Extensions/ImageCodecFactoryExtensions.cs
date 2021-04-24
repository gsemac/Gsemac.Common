using Gsemac.IO;

namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageCodecFactoryExtensions {

        public static IImageCodec Create(this IImageCodecFactory imageCodecFactory, string filename) {

            string ext = PathUtilities.GetFileExtension(filename);

            if (string.IsNullOrWhiteSpace(ext))
                return null;

            return imageCodecFactory.Create(FileFormat.FromFileExtension(ext));

        }

    }

}
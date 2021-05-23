using Gsemac.IO;
using Gsemac.IO.Extensions;
using System.IO;

namespace Gsemac.Drawing.Extensions {

    public static class ImageFactoryExtensions {

        public static IImage FromFile(this IImageFactory imageFactory, string filePath) {

            IFileFormat imageFormat = FileFormatFactory.Default.FromFile(filePath);

            using (Stream stream = File.OpenRead(filePath))
                return imageFactory.FromStream(stream, imageFormat);

        }

    }

}
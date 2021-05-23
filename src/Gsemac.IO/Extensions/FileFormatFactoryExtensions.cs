using System.IO;

namespace Gsemac.IO.Extensions {

    public static class FileFormatFactoryExtensions {

        public static IFileFormat FromMimeType(this IFileFormatFactory fileFormatFactory, string mimeType) {

            return fileFormatFactory.FromMimeType(new MimeType(mimeType));

        }
        public static IFileFormat FromFile(this IFileFormatFactory fileFormatFactory, string filePath) {

            using (Stream stream = File.OpenRead(filePath))
                return fileFormatFactory.FromStream(stream) ?? fileFormatFactory.FromFileExtension(filePath);

        }

    }

}
namespace Gsemac.IO.Extensions {

    public static class FileFormatFactoryExtensions {

        public static IFileFormat FromMimeType(this IFileFormatFactory fileFormatFactory, string mimeType) {

            return fileFormatFactory.FromMimeType(new MimeType(mimeType));

        }

    }

}
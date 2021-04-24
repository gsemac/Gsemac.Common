namespace Gsemac.IO {

    public interface IFileFormatFactory {

        IFileFormat FromMimeType(IMimeType mimeType);
        IFileFormat FromFileExtension(string fileExtension);

    }

}
namespace Gsemac.Drawing.Imaging {

    public interface IImageConverter {

        void ConvertImage(string sourceFilePath, string destinationFilePath, IImageConversionOptions options);

    }

}
namespace Gsemac.Drawing.Imaging.Extensions {

    public static class ImageConverterExtensions {

        public static void ConvertImage(this IImageConverter imageConverter, string sourceFilePath, string destinationFilePath) {

            imageConverter.ConvertImage(sourceFilePath, destinationFilePath, new ImageConversionOptions());

        }

    }

}
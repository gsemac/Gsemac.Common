using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageConverter {

        IEnumerable<string> SupportedImageFormats { get; }

        void ConvertImage(string sourceFilePath, string destinationFilePath, IImageConversionOptions options);

    }

}
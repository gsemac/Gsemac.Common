using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageConverter {

        IEnumerable<string> SupportedFileTypes { get; }

        void ConvertImage(string sourceFilePath, string destinationFilePath, IImageConversionOptions options);

    }

}
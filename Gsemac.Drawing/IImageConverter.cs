using System.Collections.Generic;

namespace Gsemac.Drawing {

    public interface IImageConverter {

        IEnumerable<string> SupportedImageFormats { get; }

        bool ConvertImage(string sourceFilename, string destinationFilename, IImageConversionOptions options);

    }

}
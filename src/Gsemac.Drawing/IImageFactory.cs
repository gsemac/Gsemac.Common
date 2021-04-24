using Gsemac.IO;

namespace Gsemac.Drawing {

    public interface IImageFactory :
        IHasSupportedFileFormats {

        IImage FromFile(string filePath);

    }

}
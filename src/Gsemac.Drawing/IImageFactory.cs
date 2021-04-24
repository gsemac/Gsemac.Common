using Gsemac.IO;

namespace Gsemac.Drawing {

    public interface IImageFactory :
        IHasSupportedFileFormats {

        IImage Create(string filePath);

    }

}
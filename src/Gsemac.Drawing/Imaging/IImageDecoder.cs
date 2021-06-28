using Gsemac.IO;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageDecoder :
        IHasSupportedFileFormats {

        IImage Decode(Stream stream);

    }

}
using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageCodec :
        IImageEncoder,
        IImageDecoder,
        IHasSupportedFileFormats {
    }

}
using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageCodecFactory :
        IHasSupportedFileFormats {

        IImageCodec FromFileFormat(IFileFormat imageFormat);

    }

}
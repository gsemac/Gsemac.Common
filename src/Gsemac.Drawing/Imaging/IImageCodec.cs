using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageCodec :
        IImageEncoder,
        IImageDecoder {

        IEnumerable<IImageFormat> SupportedImageFormats { get; }

    }

}
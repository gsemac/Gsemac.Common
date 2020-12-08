using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public interface IImageCodec :
        IImageEncoder,
        IImageDecoder {

        IEnumerable<string> SupportedFileTypes { get; }

    }

}
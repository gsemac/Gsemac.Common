#if NETFRAMEWORK

using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageDecoder {

        Image Decode(Stream stream);

    }

}

#endif
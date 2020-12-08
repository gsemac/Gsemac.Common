#if NETFRAMEWORK

using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoder {

        void Encode(Image image, Stream stream, IImageEncoderOptions options);

    }

}

#endif
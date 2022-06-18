using Gsemac.IO;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoder :
        IHasSupportedFileFormats {

        void Encode(IImage image, Stream stream, IImageEncoderOptions options);

    }

}
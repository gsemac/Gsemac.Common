using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageEncoder {

        void Encode(IImage image, Stream stream, IImageEncoderOptions options);

    }

}
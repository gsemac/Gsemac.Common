using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System.IO;

namespace Gsemac.Drawing {

    public interface IImageFactory :
        IHasSupportedFileFormats {

        IImage FromStream(Stream stream, IDecoderOptions options);

    }

}
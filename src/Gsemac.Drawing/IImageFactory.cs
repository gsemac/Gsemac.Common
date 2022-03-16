using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System.IO;

#if NETFRAMEWORK
using System.Drawing;
#endif

namespace Gsemac.Drawing {

    public interface IImageFactory :
        IHasSupportedFileFormats {

        IImage FromStream(Stream stream, IImageDecoderOptions options);

#if NETFRAMEWORK
        IImage FromBitmap(Image image, IBitmapToImageOptions options);
#endif

    }

}
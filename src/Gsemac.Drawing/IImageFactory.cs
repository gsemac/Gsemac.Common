using Gsemac.IO;
using System.IO;

#if NETFRAMEWORK
using Gsemac.Drawing.Imaging;
using System.Drawing;
#endif

namespace Gsemac.Drawing {

    public interface IImageFactory :
        IHasSupportedFileFormats {

        IImage FromStream(Stream stream, IFileFormat imageFormat = null);

#if NETFRAMEWORK
        IImage FromBitmap(Image image, IBitmapToImageOptions options = null);
#endif

    }

}
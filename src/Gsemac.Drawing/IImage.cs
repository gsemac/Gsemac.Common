using Gsemac.Drawing.Imaging;
using System;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing {

    public interface IImage :
        IDisposable {

        int Width { get; }
        int Height { get; }
        Size Size { get; }
        IImageFormat Format { get; }
        IImageCodec Codec { get; }

#if NETFRAMEWORK
        Bitmap ToBitmap(bool disposeOriginal = false);
#endif

    }

}
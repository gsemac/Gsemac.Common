using Gsemac.IO;
using System;
using System.Drawing;

namespace Gsemac.Drawing.Imaging {

    public interface IImage :
        IDisposable {

        int Width { get; }
        int Height { get; }
        Size Size { get; }
        IFileFormat Format { get; }
        IImageCodec Codec { get; }

#if NETFRAMEWORK
        Bitmap ToBitmap(bool disposeOriginal = false);
#endif

    }

}
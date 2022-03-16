using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gsemac.Drawing {

    public interface IImage :
        IDisposable {

        IAnimationInfo Animation { get; }
        int Width { get; }
        int Height { get; }
        Size Size { get; }
        IFileFormat Format { get; }
        IImageCodec Codec { get; }

        IEnumerable<IImage> GetFrames();

        IImage Clone();

#if NETFRAMEWORK
        Bitmap ToBitmap();
#endif

    }

}
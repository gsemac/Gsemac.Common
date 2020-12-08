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

        void Save(Stream stream);
        void Save(Stream stream, IImageFormat imageFormat, IImageEncoderOptions encoderOptions);

#if NETFRAMEWORK
        Bitmap ToBitmap(bool disposeOriginal = false);
#endif

    }

}
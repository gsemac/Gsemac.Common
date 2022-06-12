using Gsemac.Drawing.Imaging;
using Gsemac.IO;
using System;
using System.Drawing;

namespace Gsemac.Drawing {

    public interface IImage :
        IDisposable {

        int Width { get; }
        int Height { get; }
        Size Size { get; }
        IFileFormat Format { get; }
        IImageCodec Codec { get; }

        TimeSpan AnimationDelay { get; }
        int AnimationIterations { get; }
        int FrameCount { get; }

        IImage Clone();

        Bitmap ToBitmap();

    }

}
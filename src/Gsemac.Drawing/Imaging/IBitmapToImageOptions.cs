#if NETFRAMEWORK

using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IBitmapToImageOptions {

        IFileFormat Format { get; }
        IImageCodec Codec { get; }

    }

}

#endif
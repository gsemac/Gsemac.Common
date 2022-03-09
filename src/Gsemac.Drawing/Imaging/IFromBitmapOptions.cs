#if NETFRAMEWORK

using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IFromBitmapOptions {

        IFileFormat Format { get; }
        IImageCodec Codec { get; }

    }

}

#endif
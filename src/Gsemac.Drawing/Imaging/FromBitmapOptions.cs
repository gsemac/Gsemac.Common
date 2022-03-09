#if NETFRAMEWORK

using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class FromBitmapOptions :
        IFromBitmapOptions {

        public IFileFormat Format { get; set; }
        public IImageCodec Codec { get; set; }

        public static FromBitmapOptions Default => new FromBitmapOptions();

    }

}

#endif
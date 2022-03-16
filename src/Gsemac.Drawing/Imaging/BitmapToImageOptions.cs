#if NETFRAMEWORK

using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class BitmapToImageOptions :
        IBitmapToImageOptions {

        public IFileFormat Format { get; set; }
        public IImageCodec Codec { get; set; }

        public static BitmapToImageOptions Default => new BitmapToImageOptions();

    }

}

#endif
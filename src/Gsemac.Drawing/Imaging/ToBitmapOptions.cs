#if NETFRAMEWORK

namespace Gsemac.Drawing.Imaging {

    public class ToBitmapOptions :
        IToBitmapOptions {

        public bool DisposeSourceImage { get; set; } = false;

        public static ToBitmapOptions Default => new ToBitmapOptions();

    }

}

#endif
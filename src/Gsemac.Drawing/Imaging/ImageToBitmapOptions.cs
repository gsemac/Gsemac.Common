#if NETFRAMEWORK

namespace Gsemac.Drawing.Imaging {

    public class ImageToBitmapOptions :
        IImageToBitmapOptions {

        public bool DisposeSourceImage { get; set; } = false;

        public static ImageToBitmapOptions Default => new ImageToBitmapOptions();

    }

}

#endif
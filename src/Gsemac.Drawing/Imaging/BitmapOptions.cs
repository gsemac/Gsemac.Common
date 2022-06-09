namespace Gsemac.Drawing.Imaging {

    public class BitmapOptions :
        IBitmapOptions {

        public bool DisposeSourceImage { get; set; } = false;

        public static BitmapOptions Default => new BitmapOptions();

    }

}
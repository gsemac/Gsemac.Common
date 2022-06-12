using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class ImageDecoderOptions :
        IImageDecoderOptions {

        // Public members

        public IFileFormat Format { get; set; }
        public ImageDecoderMode Mode { get; set; } = ImageDecoderMode.Full;

        public static ImageDecoderOptions Default => new ImageDecoderOptions();

        public ImageDecoderOptions() { }
        public ImageDecoderOptions(IImageDecoderOptions options) {

            Format = options.Format;
            Mode = options.Mode;

        }

    }

}
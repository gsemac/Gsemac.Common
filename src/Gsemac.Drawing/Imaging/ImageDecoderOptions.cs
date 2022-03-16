using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class ImageDecoderOptions :
        IImageDecoderOptions {

        // Public members

        public IFileFormat Format { get; set; }
        public int? FrameIndex { get; set; }
        public int? FrameCount { get; set; }

        public static ImageDecoderOptions Default => new ImageDecoderOptions();

        public ImageDecoderOptions() { }
        public ImageDecoderOptions(IImageDecoderOptions options) {

            Format = options.Format;
            FrameIndex = options.FrameIndex;
            FrameCount = options.FrameCount;

        }

    }

}
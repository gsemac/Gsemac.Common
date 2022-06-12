using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public class DecoderOptions :
        IDecoderOptions {

        // Public members

        public IFileFormat Format { get; set; }
        public DecoderMode Mode { get; set; } = DecoderMode.Full;

        public static DecoderOptions Default => new DecoderOptions();

        public DecoderOptions() { }
        public DecoderOptions(IDecoderOptions options) {

            Format = options.Format;
            Mode = options.Mode;

        }

    }

}
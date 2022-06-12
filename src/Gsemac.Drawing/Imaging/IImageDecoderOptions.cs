using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageDecoderOptions {

        IFileFormat Format { get; }
        ImageDecoderMode Mode { get; }

    }

}
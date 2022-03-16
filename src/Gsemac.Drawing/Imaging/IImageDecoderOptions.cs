using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageDecoderOptions {

        IFileFormat Format { get; }
        int? FrameIndex { get; }
        int? FrameCount { get; }

    }

}
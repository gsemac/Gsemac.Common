using Gsemac.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IDecoderOptions {

        IFileFormat Format { get; }
        DecoderMode Mode { get; }

    }

}
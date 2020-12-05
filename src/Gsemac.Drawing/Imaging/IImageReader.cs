#if NETFRAMEWORK

using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageReader {

        IEnumerable<string> SupportedFileTypes { get; }

        Image ReadImage(Stream stream);
        void SaveImage(Image image, Stream stream, IImageEncoderOptions options);

    }

}

#endif
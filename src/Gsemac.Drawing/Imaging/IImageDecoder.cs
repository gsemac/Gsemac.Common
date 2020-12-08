using System.IO;

namespace Gsemac.Drawing.Imaging {

    public interface IImageDecoder {

        IImage Decode(Stream stream);

    }

}
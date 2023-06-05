using Gsemac.IO;
using System.IO;

namespace Gsemac.Net {

    public interface IDataUrl {

        IMimeType MimeType { get; }
        bool IsBase64Encoded { get; }
        int DataLength { get; }

        Stream GetDataStream();

    }

}
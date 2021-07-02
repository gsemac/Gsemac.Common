using System.IO;
using System.Text;

namespace Gsemac.IO.Compression {

    public interface IArchiveOptions {

        string Comment { get; }
        string Password { get; }
        bool EncryptHeaders { get; }
        CompressionLevel CompressionLevel { get; }
        Encoding Encoding { get; }
        bool LeaveStreamOpen { get; }
        FileAccess FileAccess { get; }

    }

}
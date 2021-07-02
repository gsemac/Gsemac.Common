using System.IO;
using System.Text;

namespace Gsemac.IO.Compression {

    public class ArchiveOptions :
        IArchiveOptions {

        public string Comment { get; set; }
        public string Password { get; set; }
        public bool EncryptHeaders { get; set; } = false;
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool LeaveStreamOpen { get; set; } = false;
        public FileAccess FileAccess { get; set; } = FileAccess.ReadWrite;

        public static ArchiveOptions Default => new ArchiveOptions();

    }

}
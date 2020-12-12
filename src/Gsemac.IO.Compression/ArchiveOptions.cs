using System.Text;

namespace Gsemac.IO.Compression {

    public class ArchiveOptions :
        IArchiveOptions {

        public string Comment { get; set; }
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Maximum;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

    }

}
using System.Collections.Generic;

namespace Gsemac.IO.Compression {

    public class ZipFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] { ".zip", ".cbz", ".epub" };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0x50, 0x4B, 0x03, 0x04),
            new FileSignature(0x50, 0x4B, 0x05, 0x06),
            new FileSignature(0x50, 0x4B, 0x07, 0x08),
        };
        public override string MimeType => "application/zip";

    }

}
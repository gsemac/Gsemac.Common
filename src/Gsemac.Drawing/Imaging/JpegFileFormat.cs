using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public class JpegFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] { ".jpeg", ".jpg" };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0xFF, 0xD8, 0xFF, 0xD8),
            new FileSignature(0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01),
            new FileSignature(0xFF, 0xD8, 0xFF, 0xEE),
            new FileSignature(0xFF, 0xD8, 0xFF, 0xE1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00),
        };
        public override string MimeType => "image/jpeg";

    }

}
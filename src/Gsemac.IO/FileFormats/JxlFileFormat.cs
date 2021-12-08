using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class JxlFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".jxl" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0xFF, 0x0A),
            new FileSignature(0x00, 0x00, 0x00, 0x0C, 0x4A, 0x58, 0x4C, 0x20, 0x0D, 0x0A, 0x87, 0x0A),
        };
        public override IMimeType MimeType => new MimeType("image/jxl");

    }

}
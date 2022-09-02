using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class PdfFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".pdf" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x25, 0x50, 0x44, 0x46, 0x2D),
        };
        public override IMimeType MimeType => new MimeType("application/pdf");

    }

}
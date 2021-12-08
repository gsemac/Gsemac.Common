using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class AvifFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".avif" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66) { Offset = 4 },
        };
        public override IMimeType MimeType => new MimeType("image/avif");

    }

}
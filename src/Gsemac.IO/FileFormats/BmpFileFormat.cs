using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class BmpFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".bmp", ".dib" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x42, 0x4D),
        };
        public override IMimeType MimeType => new MimeType("image/bmp");

    }

}
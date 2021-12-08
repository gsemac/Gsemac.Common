using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class GifFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".gif" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x47, 0x49, 0x46, 0x38, 0x37, 0x61),
            new FileSignature(0x47, 0x49, 0x46, 0x38, 0x39, 0x61),
        };
        public override IMimeType MimeType => new MimeType("image/gif");

    }

}
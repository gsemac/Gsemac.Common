using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class WmfFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".wmf" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0xD7, 0xCD, 0xC6, 0x9A),
        };
        public override IMimeType MimeType => new MimeType("image/wmf");

    }

}
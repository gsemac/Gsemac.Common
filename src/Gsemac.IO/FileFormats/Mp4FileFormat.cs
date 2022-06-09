using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class MP4FileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".mp4" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D) { Offset = 4 },
        };
        public override IMimeType MimeType => new MimeType("video/mp4");

    }

}
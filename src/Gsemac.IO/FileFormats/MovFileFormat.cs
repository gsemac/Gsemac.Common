using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class MovFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".mov",
            ".movie",
            ".qt",
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20) {
                Offset = 4,
            }, // ftypqt
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("video/quicktime"),
        };
        public override string Name => "QuickTime Video";

    }

}
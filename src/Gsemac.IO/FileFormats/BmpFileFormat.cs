using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class BmpFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".bmp",
            ".dib"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x42, 0x4D),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("image/bmp")
        };
        public override string Name => "Bitmap";

    }

}
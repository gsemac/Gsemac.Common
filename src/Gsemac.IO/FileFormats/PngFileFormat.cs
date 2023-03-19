using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class PngFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".png"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
             new MimeType("image/png")
        };

    }

}
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class TiffFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".tiff",
            ".tif"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x49, 0x49, 0x2A, 0x00),
            new FileSignature(0x4D, 0x4D, 0x00, 0x2A),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("image/tiff")
        };

    }

}
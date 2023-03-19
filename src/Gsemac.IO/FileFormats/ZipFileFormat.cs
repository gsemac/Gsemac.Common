using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public class ZipFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".zip",
            ".cbz",
            ".epub"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x50, 0x4B, 0x03, 0x04),
            new FileSignature(0x50, 0x4B, 0x05, 0x06),
            new FileSignature(0x50, 0x4B, 0x07, 0x08),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("application/zip")
        };

    }

}
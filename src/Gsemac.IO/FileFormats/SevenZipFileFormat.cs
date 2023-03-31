using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public class SevenZipFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".7z",
            ".cb7"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("application/x-7z-compressed")
        };
        public override string Name => "7z Archive";

    }

}
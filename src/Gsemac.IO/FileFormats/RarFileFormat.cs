using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public class RarFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] {
            ".rar",
            ".cbr"
        };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00), // RAR archive version 1.50 onwards
            new FileSignature(0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00), // RAR archive version 5.0 onwards
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("application/x-rar-compressed")
        };

    }

}
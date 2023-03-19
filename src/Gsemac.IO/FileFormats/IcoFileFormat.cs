using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class IcoFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".ico"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x00, 0x00, 0x01, 0x00),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("image/vnd.microsoft.icon"),
            new MimeType("image/x-icon"),
            new MimeType("image/ico"),
        };

    }

}
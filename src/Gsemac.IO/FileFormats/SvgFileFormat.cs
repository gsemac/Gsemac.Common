using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class SvgFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".svg"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new TextFileSignature("<svg ", FileSignatureOptions.IgnoreLeadingWhiteSpace | FileSignatureOptions.CaseInsensitive),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("image/svg+xml")
        };
        public override string Name => "SVG Graphics";

    }

}
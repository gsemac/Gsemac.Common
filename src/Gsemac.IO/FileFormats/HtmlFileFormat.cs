using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class HtmlFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".html",
            ".htm"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new TextFileSignature("<html ", FileSignatureOptions.IgnoreLeadingWhiteSpace | FileSignatureOptions.CaseInsensitive),
            new TextFileSignature("<!doctype html>", FileSignatureOptions.IgnoreLeadingWhiteSpace | FileSignatureOptions.CaseInsensitive),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("text/html")
        };
        public override string Name => "HTML Document";

    }

}
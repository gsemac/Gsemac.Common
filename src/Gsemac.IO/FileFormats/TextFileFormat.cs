using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO.FileFormats {

    public sealed class TextFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".txt",
        };
        public override IEnumerable<IFileSignature> Signatures => Enumerable.Empty<IFileSignature>(); // Detect BOM marks?
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("text/plain")
        };
        public override string Name => "Text Document";

    }

}
using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class AnyFileFormat :
        FileFormatBase {

        // Public members

        public override IEnumerable<string> Extensions => new[] {
            "*"
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("*/*")
        };
        public override string Name => "All Files";

    }

}
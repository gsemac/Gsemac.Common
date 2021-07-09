using System.Collections.Generic;

namespace Gsemac.IO {

    public sealed class AnyFileFormat :
        FileFormatBase {

        // Public members

        public override IEnumerable<string> Extensions => new[] { "*" };
        public override IMimeType MimeType => new MimeType("*/*");

    }

}
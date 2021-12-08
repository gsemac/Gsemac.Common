using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class AvifFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".avif" };
        public override IMimeType MimeType => new MimeType("image/avif");

    }

}
using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public sealed class AvifFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".avif" };
        public override IMimeType MimeType => new MimeType("image/avif");

    }

}
using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public sealed class IcoFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] { ".ico" };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x00, 0x00, 0x01, 0x00),
        };
        public override IMimeType MimeType => new MimeType("image/vnd.microsoft.icon");

    }

}
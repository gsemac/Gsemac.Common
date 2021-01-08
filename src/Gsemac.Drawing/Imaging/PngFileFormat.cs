using Gsemac.IO;
using System.Collections.Generic;

namespace Gsemac.Drawing.Imaging {

    public class PngFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] { ".png" };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A),
        };
        public override string MimeType => "image/png";

    }

}
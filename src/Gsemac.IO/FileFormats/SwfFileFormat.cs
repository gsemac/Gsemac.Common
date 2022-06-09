using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public class SwfFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] { ".swf" };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0x43, 0x57, 0x53), // CWS
            new FileSignature(0x46, 0x57, 0x53), // FWS
        };
        public override IMimeType MimeType => new MimeType("application/x-shockwave-flash");

    }

}
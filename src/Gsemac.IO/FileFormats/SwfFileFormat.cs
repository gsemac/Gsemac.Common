using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public class SwfFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".swf"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x43, 0x57, 0x53), // CWS
            new FileSignature(0x46, 0x57, 0x53), // FWS
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("application/x-shockwave-flash"),
            new MimeType("application/vnd.adobe.flash.movie"),
        };
        public override string Name => "Adobe Flash";

    }

}
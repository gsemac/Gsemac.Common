﻿using System.Collections.Generic;

namespace Gsemac.IO.Compression {

    public class SevenZipFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new string[] { ".7z" };
        public override IEnumerable<IFileSignature> Signatures => new IFileSignature[] {
            new FileSignature(0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C),
        };
        public override string MimeType => "application/x-7z-compressed";

    }

}
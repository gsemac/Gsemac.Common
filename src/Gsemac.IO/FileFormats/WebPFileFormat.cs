﻿using System.Collections.Generic;

namespace Gsemac.IO.FileFormats {

    public sealed class WebPFileFormat :
        FileFormatBase {

        public override IEnumerable<string> Extensions => new[] {
            ".webp"
        };
        public override IEnumerable<IFileSignature> Signatures => new[] {
            new FileSignature(0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x57, 0x45, 0x42, 0x50),
        };
        public override IEnumerable<IMimeType> MimeTypes => new[] {
            new MimeType("image/webp")
        };
        public override string Name => "WebP Image";

    }

}
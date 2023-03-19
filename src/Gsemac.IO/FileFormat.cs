using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileFormat :
        FileFormatBase {

        // Public members

        public override IEnumerable<string> Extensions { get; } = Enumerable.Empty<string>();
        public override IEnumerable<IFileSignature> Signatures { get; } = Enumerable.Empty<IFileSignature>();
        public override IEnumerable<IMimeType> MimeTypes { get; }

        internal FileFormat(string fileExtension) {

            if (fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase)) {

                MimeTypes = new[] {
                    new MimeType("text/plain")
                };

            }
            else {

                MimeTypes = new[] {
                    new MimeType("application/octet-stream")
                };

            }

            Extensions = new string[] {
                fileExtension
            };

        }

    }

}
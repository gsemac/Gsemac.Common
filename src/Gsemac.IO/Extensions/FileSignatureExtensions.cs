using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO.Extensions {

    public static class FileSignatureExtensions {

        // Public members

        public static bool IsMatch(this IFileSignature fileSignature, IEnumerable<byte> bytes) {

            using (Stream ms = new MemoryStream(bytes.ToArray()))
                return fileSignature.IsMatch(ms);

        }

    }

}
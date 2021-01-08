using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileSignature :
        IFileSignature {

        // Public members

        public byte? this[int index] => signatureBytes[index];

        public FileSignature(params byte?[] signatureBytes) {

            this.signatureBytes = signatureBytes;

        }
        public FileSignature(params int?[] signatureBytes) :
            this(signatureBytes.Select(i => (byte?)i).ToArray()) {
        }

        public IEnumerator<byte?> GetEnumerator() {

            return ((IEnumerable<byte?>)signatureBytes).GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

        }

        // Private members

        private readonly byte?[] signatureBytes;

    }

}
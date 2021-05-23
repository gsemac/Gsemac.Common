using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gsemac.IO {

    public class FileSignature :
        IFileSignature {

        // Public members

        public byte? this[int index] => signatureBytes[index];

        public int Length => signatureBytes.Length;

        public FileSignature(params byte?[] signatureBytes) {

            this.signatureBytes = signatureBytes;

        }
        public FileSignature(params int?[] signatureBytes) :
            this(signatureBytes.Select(i => (byte?)i).ToArray()) {
        }

        public bool IsMatch(IEnumerable<byte> bytes) {

            return signatureBytes.Zip(bytes, (first, second) => Tuple.Create(first, second))
                .All(pair => !pair.Item1.HasValue || pair.Item1 == pair.Item2);

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
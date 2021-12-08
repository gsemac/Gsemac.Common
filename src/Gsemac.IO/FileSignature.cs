using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.IO {

    public class FileSignature :
        IFileSignature {

        // Public members

        public byte? this[int index] => signatureBytes[index];

        public int Length => signatureBytes.Length;

        public FileSignature(params byte?[] signatureBytes) {

            if (signatureBytes is null)
                throw new ArgumentNullException(nameof(signatureBytes));

            this.signatureBytes = signatureBytes;

        }
        public FileSignature(params int?[] signatureBytes) :
            this(signatureBytes.Select(i => (byte?)i).ToArray()) {
        }

        public virtual bool IsMatch(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            foreach (byte? signatureBytes in signatureBytes) {

                int nextStreamByte = stream.ReadByte();

                if (nextStreamByte < 0)
                    return false;

                if (signatureBytes is null)
                    continue;

                if (signatureBytes != nextStreamByte)
                    return false;

            }

            return true;

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
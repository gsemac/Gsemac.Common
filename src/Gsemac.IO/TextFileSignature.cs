using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO {

    public abstract class TextFileSignature :
        FileSignature {

        // Public members

        public override bool IsMatch(Stream stream) {

            // Skip all whitespace in the stream before attempting to find a match.
            // TODO: Detect UTF8 encoding from BOM

            using (TextReader reader = new UnbufferedStreamReader(stream, encoding)) {

                int nextChar;

                do {

                    nextChar = reader.Read();

                    if (nextChar != -1 && !char.IsWhiteSpace((char)nextChar)) {

                        // We've detected a non-whitespace character.

                        byte[] charBytes = encoding.GetBytes(((char)nextChar).ToString());

                        using (Stream charStream = new MemoryStream(charBytes))
                        using (Stream fullStream = new ConcatStream(charStream, stream))
                            return base.IsMatch(fullStream);

                    }

                } while (nextChar != -1);

                // If we get here, we reached the end of the stream and found nothing but whitespace.

                return false;

            }

        }

        // Protected members

        protected TextFileSignature(string signature) :
            this(signature, DefaultEncoding) {
        }
        protected TextFileSignature(string signature, Encoding encoding) :
            base(GetSignatureBytes(signature, encoding)) {

            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            this.encoding = encoding;

        }

        // Private members

        private readonly Encoding encoding = DefaultEncoding;

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private static byte?[] GetSignatureBytes(string signature, Encoding encoding) {

            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            if (string.IsNullOrEmpty(signature))
                return Polyfills.System.Array.Empty<byte?>();

            return encoding.GetBytes(signature).Cast<byte?>().ToArray();

        }

    }

}
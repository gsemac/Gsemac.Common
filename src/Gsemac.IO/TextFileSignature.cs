using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.IO {

    public sealed class TextFileSignature :
        FileSignature {

        // Public members

        public TextFileSignature(string signature) :
            this(signature, FileSignatureOptions.Default) {
        }
        public TextFileSignature(string signature, FileSignatureOptions options) :
            this(signature, DefaultEncoding, options) {
        }
        public TextFileSignature(string signature, Encoding encoding) :
             this(signature, encoding, FileSignatureOptions.Default) {
        }
        public TextFileSignature(string signature, Encoding encoding, FileSignatureOptions options) :
         base(GetSignatureBytes(signature, encoding, options)) {

            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            this.signature = signature;
            this.encoding = encoding;
            this.options = options;

        }

        public override bool IsMatch(Stream stream) {

            // Skip all whitespace in the stream before attempting to find a match.
            // TODO: Detect UTF8 encoding from BOM

            using (TextReader reader = new UnbufferedStreamReader(stream, encoding)) {

                int nextChar;

                do {

                    nextChar = reader.Read();

                    if (nextChar != -1 && (!options.HasFlag(FileSignatureOptions.IgnoreLeadingWhiteSpace) || !char.IsWhiteSpace((char)nextChar))) {

                        // We've detected a non-whitespace character, so begin the comparison.

                        foreach (char signatureChar in signature) {

                            if (nextChar < 0)
                                return false;

                            if (options.HasFlag(FileSignatureOptions.CaseInsensitive))
                                nextChar = char.ToLowerInvariant((char)nextChar);

                            if (signatureChar != nextChar)
                                return false;

                            nextChar = stream.ReadByte();

                        }

                        // If we got here, we reached the end of the signature and found a match.

                        return true;

                    }

                } while (nextChar != -1);

                // If we get here, we reached the end of the stream and found nothing but whitespace.

                return false;

            }

        }

        // Private members

        private readonly string signature;
        private readonly Encoding encoding = DefaultEncoding;
        private readonly FileSignatureOptions options = FileSignatureOptions.Default;

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private static byte?[] GetSignatureBytes(string signature, Encoding encoding, FileSignatureOptions options) {

            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));

            if (string.IsNullOrEmpty(signature))
                return Polyfills.System.Array.Empty<byte?>();

            if (options.HasFlag(FileSignatureOptions.CaseInsensitive))
                signature = signature.ToLowerInvariant();

            return encoding.GetBytes(signature).Cast<byte?>().ToArray();

        }

    }

}
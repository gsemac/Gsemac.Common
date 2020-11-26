using System.IO;
using System.Text;

namespace Gsemac.IO {

    public class UnbufferedStreamReader :
        TextReader {

        // Public members

        public static new UnbufferedStreamReader Null => new UnbufferedStreamReader(null);

        public Stream BaseStream => stream;
        public Encoding CurrentEncoding => encoding;
        public bool EndOfStream => endOfStream;

        public UnbufferedStreamReader(Stream stream) :
            this(stream, Encoding.UTF8) {
        }
        public UnbufferedStreamReader(Stream stream, Encoding encoding) {

            this.stream = stream;
            this.encoding = encoding;

        }

        public override void Close() {

            base.Close();

            stream.Close();

        }

        public override int Peek() {

            return PeekNextCharacter();

        }
        public override int Read() {

            return ReadNextCharacter();

        }
        public override int Read(char[] buffer, int index, int count) {

            int bytesRead = 0;

            for (int i = index; i < index + count; ++i) {

                int nextChar = Read();

                if (nextChar < 0)
                    break;

                buffer[index] = (char)nextChar;

                ++bytesRead;

            }

            return bytesRead;

        }
        public override string ReadLine() {

            // Read until we hit "\n", "\r", or "\r\n".
            // The newline sequence we encounter is consumed, but not returned.

            if (EndOfStream)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            while (!EndOfStream) {

                int nextChar = ReadNextCharacter();

                if (nextChar == '\n')
                    break;

                if (nextChar == '\r') {

                    // Consume the following newline, if present.

                    nextChar = PeekNextCharacter();

                    if (nextChar == '\n')
                        ReadNextCharacter();

                    break;

                }

                if (nextChar >= 0)
                    sb.Append((char)nextChar);

            }

            return sb.ToString();

        }
        public override string ReadToEnd() {

            // Since we're reading the entire stream, using a buffer is the same as not.

            using (StreamReader sr = new StreamReader(stream, encoding))
                return sr.ReadToEnd();

        }

        // Private members

        private readonly Stream stream;
        private readonly Encoding encoding;
        private readonly char[] buffer = new char[1];
        private bool endOfStream = false;
        private bool isBufferEmpty = true;

        private int PeekNextCharacter() {

            // The stream will be read one character at a time with the current encoding.

            if (!isBufferEmpty)
                return buffer[0];

            Decoder decoder = encoding.GetDecoder();

            int nextByte;

            while ((nextByte = stream.ReadByte()) != -1) {

                int decodedCharCount = decoder.GetChars(new[] { (byte)nextByte }, 0, 1, buffer, 0);

                if (decodedCharCount > 0) {

                    isBufferEmpty = false;

                    return buffer[0];

                }

            }

            // If we reach this point, no chars were read.

            endOfStream = true;

            return -1;

        }
        private int ReadNextCharacter() {

            int nextChar = PeekNextCharacter();

            isBufferEmpty = true;

            return nextChar;

        }

    }

}
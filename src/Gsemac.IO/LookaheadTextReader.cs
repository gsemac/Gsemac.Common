using Gsemac.Collections;
using System;
using System.IO;
using System.Text;

namespace Gsemac.IO {

    public class LookaheadTextReader :
        TextReader {

        // Public members

        public LookaheadTextReader(Stream stream) :
             this(new StreamReader(stream)) {
        }
        public LookaheadTextReader(Stream stream, Encoding encoding) :
            this(new StreamReader(stream, encoding)) {
        }
        public LookaheadTextReader(string value) :
            this(new StringReader(value)) {
        }
        public LookaheadTextReader(TextReader reader) {

            this.reader = reader;

        }

        public override int Read() {

            if (peekBuffer.Length > 0)
                return peekBuffer.Read();

            return reader.Read();

        }
        public override int Read(char[] buffer, int index, int count) {

            int charsRead = 0;

            if (peekBuffer.Length > 0) {

                charsRead += peekBuffer.Read(buffer, index, count);

                index += charsRead;
                count -= charsRead;

            }

            if (count > 0)
                charsRead += reader.Read(buffer, index, count);

            return charsRead;

        }

        public override int Peek() {

            if (peekBuffer.Length > 0)
                return peekBuffer.Peek();

            return reader.Peek();

        }
        public int Peek(char[] buffer, int index, int count) {

            // Read the requested number of chars into the peek buffer.
            // We only need to read enough that the buffer's length is equal to or greater than count.

            EnsurePeekBufferFilled(count);

            // Copy from the peek buffer to the output buffer.

            int charsReadFromPeekBuffer = Math.Min(count, buffer.Length);

            for (int i = 0; i < charsReadFromPeekBuffer; ++i)
                buffer[index + i] = peekBuffer[i];

            return charsReadFromPeekBuffer;

        }

        public override void Close() {

            reader.Close();

            base.Close();

        }

        // Private members

        private readonly TextReader reader;
        private readonly CircularBuffer<char> peekBuffer = new CircularBuffer<char>();

        private void EnsurePeekBufferFilled(int count) {

            if (peekBuffer.Length < count) {

                int charsRequired = count - peekBuffer.Length;

                char[] temp = new char[charsRequired];

                int charsRead = reader.Read(temp, 0, charsRequired);

                peekBuffer.Write(temp, 0, charsRead);

            }

        }

    }

}
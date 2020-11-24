using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Text {

    public abstract class LexerBase<T> :
         ILexer<T> {

        public bool EndOfStream => Reader.EndOfStream;

        public abstract bool ReadNextToken(out T token);
        public abstract T PeekNextToken();

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected StreamReader Reader { get; }

        protected LexerBase(Stream stream) {

            Reader = new StreamReader(stream);

        }

        protected void SkipCharacter() {

            if (!Reader.EndOfStream)
                Reader.Read();

        }
        protected void SkipWhitespace() {

            while (!Reader.EndOfStream && char.IsWhiteSpace((char)Reader.Peek()))
                Reader.Read();

        }

        protected char? PeekCharacter() {

            if (Reader.EndOfStream)
                return null;

            return (char)Reader.Peek();

        }
        protected char? ReadCharacter() {

            if (Reader.EndOfStream)
                return null;

            return (char)Reader.Read();

        }
        protected string ReadUntil(char delimiter) {

            return ReadUntilAny(new[] { delimiter });

        }
        protected string ReadUntilAny(params char[] delimiters) {

            return ReadUntilAny(delimiters, false);

        }
        protected string ReadUntilAny(char[] delimiters, bool allowEscapeSequences) {

            StringBuilder valueBuilder = new StringBuilder();
            bool insideEscapeSequence = false;

            while (!EndOfStream && ((insideEscapeSequence && allowEscapeSequences) || !delimiters.Any(c => c == (char)Reader.Peek()))) {

                char nextChar = (char)Reader.Peek();

                if (nextChar == '\\' && !insideEscapeSequence)
                    insideEscapeSequence = true;
                else
                    insideEscapeSequence = false;

                valueBuilder.Append(ReadCharacter());

            }

            return valueBuilder.ToString();

        }

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    Reader.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private bool disposedValue = false;

    }

}
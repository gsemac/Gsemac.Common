using System.Collections.Generic;
using System.IO;

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

        protected string ReadCharacter() {

            if (Reader.EndOfStream)
                return string.Empty;

            return ((char)Reader.Read()).ToString();

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
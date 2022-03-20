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
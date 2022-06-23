using System;
using System.IO;

namespace Gsemac.Text.Lexers {

    public abstract class LexerBase<T> :
         ILexer<T> {

        public abstract bool Read(out T token);
        public abstract T Peek();

        public void Dispose() {

            Dispose(true);

        }

        // Protected members

        protected TextReader Reader { get; }

        protected LexerBase(Stream stream) :
            this(new StreamReader(stream)) {
        }
        protected LexerBase(TextReader reader) {

            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            Reader = reader;

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
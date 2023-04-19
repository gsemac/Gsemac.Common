using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gsemac.Text.Lexers {

    public abstract class LexerBase<LexerTokenT> :
         ILexer<LexerTokenT> {

        // Public members

        public bool Read(out LexerTokenT token) {

            if (!tokens.Any())
                ReadNext(tokens);

            if (tokens.Any()) {

                token = tokens.Dequeue();

                return true;

            }
            else {

                token = default;

                return false;

            }

        }
        public LexerTokenT Peek() {

            if (!tokens.Any())
                ReadNext(tokens);

            return tokens.Any() ? tokens.Peek() : default;

        }

        public void Dispose() {

            Dispose(true);

        }

        public IEnumerator<LexerTokenT> GetEnumerator() {

            while (Read(out LexerTokenT token))
                yield return token;

        }
        IEnumerator IEnumerable.GetEnumerator() {

            return GetEnumerator();

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

        protected abstract void ReadNext(Queue<LexerTokenT> tokens);

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
        private readonly Queue<LexerTokenT> tokens = new Queue<LexerTokenT>();

    }

}
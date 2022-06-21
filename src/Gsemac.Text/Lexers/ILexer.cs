using System;

namespace Gsemac.Text.Lexers {

    public interface ILexer<T> :
        IDisposable {

        bool EndOfStream { get; }

        bool ReadToken(out T token);
        T Peek();

    }

}
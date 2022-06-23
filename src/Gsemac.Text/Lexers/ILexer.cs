using System;

namespace Gsemac.Text.Lexers {

    public interface ILexer<T> :
        IDisposable {

        bool Read(out T token);
        T Peek();

    }

}
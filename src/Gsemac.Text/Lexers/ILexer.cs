using System;
using System.Collections.Generic;

namespace Gsemac.Text.Lexers {

    public interface ILexer<T> :
        IEnumerable<T>,
        IDisposable {

        bool Read(out T token);
        T Peek();

    }

}
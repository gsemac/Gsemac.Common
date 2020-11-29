using System;

namespace Gsemac.Text {

    public interface ILexer<T> :
        IDisposable {

        bool EndOfStream { get; }

        bool ReadNextToken(out T token);
        T PeekNextToken();

    }

}
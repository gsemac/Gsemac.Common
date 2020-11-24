using System;

namespace Gsemac.Text {

    public interface ILexer<T> :
        IDisposable {

        bool EndOfStream { get; }

        bool Read(out T token);
        T Peek();

    }

}
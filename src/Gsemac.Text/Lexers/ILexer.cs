using System;
using System.Collections.Generic;

namespace Gsemac.Text.Lexers {

    public interface ILexer<LexerTokenT> :
        IEnumerable<LexerTokenT>,
        IDisposable {

        bool Read(out LexerTokenT token);
        LexerTokenT Peek();

    }

}
using System;

namespace Gsemac.Text.Lexers {

    public interface ILexerToken<LexerTokenEnumT> where LexerTokenEnumT : Enum {

        LexerTokenEnumT Type { get; }
        string Value { get; }

    }

}
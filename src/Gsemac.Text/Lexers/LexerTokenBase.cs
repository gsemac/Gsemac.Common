using System;

namespace Gsemac.Text.Lexers {

    public abstract class LexerTokenBase<TokenTypeT> :
        ILexerToken<TokenTypeT> where TokenTypeT : Enum {

        // Public members

        public TokenTypeT Type { get; }
        public string Value { get; }

        public override string ToString() {

            return $"{Type}: {Value}";

        }

        // Protected members

        protected LexerTokenBase(TokenTypeT type, string value) {

            Type = type;
            Value = value;

        }

    }

}
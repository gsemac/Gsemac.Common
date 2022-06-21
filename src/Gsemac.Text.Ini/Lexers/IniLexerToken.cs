namespace Gsemac.Text.Ini.Lexers {

    internal class IniLexerToken :
        IIniLexerToken {

        // Public members

        public IniLexerTokenType Type { get; }
        public string Value { get; }

        public IniLexerToken(IniLexerTokenType type, string value) {

            Type = type;
            Value = value;

        }

        public override string ToString() {

            return $"{Type}: {Value}";

        }

    }

}
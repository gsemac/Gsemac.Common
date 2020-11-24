namespace Gsemac.Text.Ini {

    public class IniLexerToken :
        IIniLexerToken {

        // Public members

        public IniLexerTokenType Type { get; }
        public string Value { get; }

        public IniLexerToken(IniLexerTokenType type, string value) {

            this.Type = type;
            this.Value = value;

        }

        public override string ToString() {

            return $"{Type}: {Value}";

        }

    }

}
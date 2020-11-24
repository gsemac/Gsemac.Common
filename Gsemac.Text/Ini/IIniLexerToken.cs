namespace Gsemac.Text.Ini {

    public enum IniLexerTokenType {
        SectionStart, // [
        SectionName,
        SectionEnd, // ]
        PropertyName,
        PropertyValueSeparator, // =
        PropertyValue
    }

    public interface IIniLexerToken {

        // Public members

        IniLexerTokenType Type { get; }
        string Value { get; }

    }

}
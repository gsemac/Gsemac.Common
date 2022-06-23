namespace Gsemac.Text.Ini.Lexers {

    internal enum IniLexerTokenType {
        SectionNameStart, // [
        SectionName,
        SectionNameEnd, // ]
        PropertyName,
        PropertyValueSeparator, // =, :
        PropertyValue,
        CommentMarker, // ;, #
        Comment
    }

    internal interface IIniLexerToken {

        // Public members

        IniLexerTokenType Type { get; }
        string Value { get; }

    }

}
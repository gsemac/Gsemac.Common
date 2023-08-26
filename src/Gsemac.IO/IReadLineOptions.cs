namespace Gsemac.IO {

    public interface IReadLineOptions {

        bool AllowEscapedDelimiters { get; }
        bool BreakOnNewLine { get; }
        bool ConsumeDelimiter { get; }

        char EscapeCharacter { get; }

    }

}
namespace Gsemac.IO {

    public class ReadLineOptions :
        IReadLineOptions {

        // Public members

        public bool AllowEscapedDelimiters { get; set; } = false;
        public bool BreakOnNewLine { get; set; } = true;
        public bool ConsumeDelimiter { get; set; } = true;

        public char EscapeCharacter { get; set; } = '\\';

        public static ReadLineOptions Default => new ReadLineOptions();

    }

}
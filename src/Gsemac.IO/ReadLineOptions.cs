namespace Gsemac.IO {

    public class ReadLineOptions :
        IReadLineOptions {

        // Public members

        public bool BreakOnNewLine { get; set; } = true;
        public bool ConsumeDelimiter { get; set; } = true;
        public bool AllowEscapedDelimiters { get; set; } = false;

        public static ReadLineOptions Default => new ReadLineOptions();

    }

}
namespace Gsemac.Text {

    public sealed class StringSplitOptionsEx :
        IStringSplitOptionsEx {

        // Public members

        public bool AllowEnclosedDelimiters { get; set; } = false;
        public bool RemoveEmptyEntries { get; set; } = false;
        public bool SplitAfterDelimiter { get; set; } = false;
        public bool SplitBeforeDelimiter { get; set; } = false;
        public bool TrimEntries { get; set; } = false;

        public static StringSplitOptionsEx Default => new StringSplitOptionsEx();

    }

}
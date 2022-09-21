namespace Gsemac.Core {

    public class RangeFormattingOptions :
        IRangeFormattingOptions {

        // Public members

        public bool Normalize { get; set; } = false;

        public static RangeFormattingOptions Default => new RangeFormattingOptions();

    }

}
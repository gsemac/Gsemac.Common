namespace Gsemac.Core {

    public class RangeParsingOptions :
        IRangeParsingOptions {

        public bool AllowNegativeNumbers { get; set; } = true;
        public bool AllowDashedRanges { get; set; } = false;
        public bool AllowBoundedRanges { get; set; } = true;
        public bool IgnoreInvalidRanges { get; set; } = false;

        public static RangeParsingOptions Default => new RangeParsingOptions();

    }

}
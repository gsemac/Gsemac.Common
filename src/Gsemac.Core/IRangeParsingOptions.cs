namespace Gsemac.Core {

    public interface IRangeParsingOptions {

        bool AllowNegativeNumbers { get; }
        bool AllowDashedRanges { get; }
        bool AllowBoundedRanges { get; }
        bool IgnoreInvalidRanges { get; }

    }

}
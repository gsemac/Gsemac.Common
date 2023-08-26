namespace Gsemac.Text {

    public interface IStringSplitOptionsEx {

        bool AllowEnclosedDelimiters { get; }
        bool RemoveEmptyEntries { get; }
        bool SplitAfterDelimiter { get; }
        bool SplitBeforeDelimiter { get; }
        bool TrimEntries { get; }


    }

}
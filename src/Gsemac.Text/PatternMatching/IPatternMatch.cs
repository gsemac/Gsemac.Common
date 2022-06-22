namespace Gsemac.Text.PatternMatching {

    public interface IPatternMatch {

        bool Success { get; }
        int Index { get; }
        int Length { get; }
        string Value { get; }

    }

}
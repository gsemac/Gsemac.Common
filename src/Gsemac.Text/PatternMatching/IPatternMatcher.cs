namespace Gsemac.Text.PatternMatching {

    public interface IPatternMatcher {

        bool IsMatch(string input);

        IPatternMatch Match(string input);

    }

}
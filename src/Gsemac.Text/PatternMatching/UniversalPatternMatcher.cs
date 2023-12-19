namespace Gsemac.Text.PatternMatching {

    internal class UniversalPatternMatcher :
        PatternMatcherBase {

        // Public members

        public override IPatternMatch Match(string input) {

            input = input ?? string.Empty;

            return new PatternMatch(0, input);

        }

    }

}
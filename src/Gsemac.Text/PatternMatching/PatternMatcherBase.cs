namespace Gsemac.Text.PatternMatching {

    public abstract class PatternMatcherBase :
        IPatternMatcher {

        // Public members

        public virtual bool IsMatch(string input) {

            return Match(input)?.Success ?? false;

        }
        public abstract IPatternMatch Match(string input);

    }

}
namespace Gsemac.Text.PatternMatching {

    public static class PatternMatcher {

        // Public members

        public static IPatternMatcher Any => new UniversalPatternMatcher();

    }

}
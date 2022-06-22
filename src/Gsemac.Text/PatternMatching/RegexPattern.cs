using System.Text.RegularExpressions;

namespace Gsemac.Text.PatternMatching {

    public class RegexPattern :
        PatternMatcherBase {

        // Public members

        public RegexPattern(string regex) {

            this.regex = new Regex(regex);

        }
        public RegexPattern(Regex regex) {

            this.regex = regex;

        }

        public override IPatternMatch Match(string input) {

            return new RegexPatternMatchAdapter(regex.Match(input));

        }

        // Private members

        private readonly Regex regex;

    }

}
using System;
using System.Text.RegularExpressions;

namespace Gsemac.Text.PatternMatching {

    public class RegexPattern :
        PatternMatcherBase {

        // Public members

        public RegexPattern(string regex) {

            this.regex = new Regex(regex);

        }
        public RegexPattern(Regex regex) {

            if (regex is null)
                throw new ArgumentNullException(nameof(regex));

            this.regex = regex;

        }

        public override IPatternMatch Match(string input) {

            return new RegexPatternMatchAdapter(regex.Match(input));

        }

        public override string ToString() {

            return regex.ToString();

        }

        // Private members

        private readonly Regex regex;

    }

}
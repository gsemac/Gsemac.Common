using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Text.PatternMatching {

    public class WildcardPattern :
        PatternMatcherBase {

        // Public members

        public const char WildcardChar = '*';

        public WildcardPattern(string pattern) :
            this(pattern, DefaultStringComparison) {
        }
        public WildcardPattern(string pattern, StringComparison stringComparison) {

            if (pattern is null)
                throw new ArgumentNullException(nameof(pattern));

            patternContainsWildcardChar = pattern.Contains(WildcardChar);

            this.pattern = pattern;
            this.stringComparison = stringComparison;

        }

        public override IPatternMatch Match(string input) {

            input = input ?? string.Empty;

            if (patternContainsWildcardChar) {

                // If the pattern is just the wildcard character, we match any string.

                if (input.Length == 1)
                    return new PatternMatch(0, input);

                // Convert the wildcard pattern to a regular expression.

                Regex regex = CreateRegex();
                Match match = regex.Match(input);

                return new RegexPatternMatchAdapter(match);

            }
            else {

                // The pattern does not contain any wildcard characters, so just check for string equality.

                bool patternAndInputBothEmpty = string.IsNullOrEmpty(input) && string.IsNullOrEmpty(pattern);
                bool isMatch = patternAndInputBothEmpty || (!string.IsNullOrEmpty(input) && input.Equals(pattern, stringComparison));

                return isMatch ?
                    new PatternMatch(0, input) :
                    PatternMatch.Failure;

            }

        }

        public override string ToString() {

            return pattern;

        }

        // Private members

        private const StringComparison DefaultStringComparison = StringComparison.OrdinalIgnoreCase;

        private readonly string pattern;
        private readonly bool patternContainsWildcardChar;
        private readonly StringComparison stringComparison;

        private Regex CreateRegex() {

            string[] split = pattern.Split(new[] { WildcardChar }, System.StringSplitOptions.None);

            RegexOptions options = RegexOptions.None;

            if (IgnoreCase())
                options |= RegexOptions.IgnoreCase;

            string regexPattern = "^" + string.Join(".*", split.Select(x => Regex.Escape(x))) + "$";
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

            return regex;

        }
        private bool IgnoreCase() {

            switch (stringComparison) {

                case StringComparison.OrdinalIgnoreCase:
                case StringComparison.InvariantCultureIgnoreCase:
                case StringComparison.CurrentCultureIgnoreCase:
                    return true;

                default:
                    return false;

            }

        }

    }

}
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public class WildcardPattern {

        // Public members

        public const char WildcardChar = '*';

        public WildcardPattern(string pattern) {

            this.pattern = pattern;

        }

        public bool IsMatch(string input) {

            string[] split = pattern.Split(WildcardChar);

            if (split.Length > 1) {

                string regexPattern = "^" + string.Join(".*", split.Select(x => Regex.Escape(x))) + "$";
                Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

                return regex.IsMatch(input);

            }
            else
                return input.Equals(pattern, StringComparison.OrdinalIgnoreCase);

        }

        public override string ToString() {

            return pattern;

        }

        // Private members

        private readonly string pattern;

    }

}
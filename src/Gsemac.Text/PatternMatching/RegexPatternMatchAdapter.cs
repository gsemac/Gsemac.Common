using System;
using System.Text.RegularExpressions;

namespace Gsemac.Text.PatternMatching {

    internal class RegexPatternMatchAdapter :
        IPatternMatch {

        // Public members

        public bool Success => match.Success;
        public int Index => match.Index;
        public int Length => match.Length;
        public string Value => match.Value;

        public RegexPatternMatchAdapter(Match match) {

            if (match is null)
                throw new ArgumentNullException(nameof(match));

            this.match = match;

        }

        // Private members

        private readonly Match match;

    }

}
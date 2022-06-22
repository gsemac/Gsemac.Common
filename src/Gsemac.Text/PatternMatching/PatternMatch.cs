namespace Gsemac.Text.PatternMatching {

    internal class PatternMatch :
        IPatternMatch {

        // Public members

        public bool Success { get; } = true;
        public int Index { get; } = 0;
        public int Length { get; } = 0;
        public string Value { get; }

        public static PatternMatch Failure => new PatternMatch();

        public PatternMatch(int index, string value) {

            Success = true;
            Index = index;
            Length = value?.Length ?? 0;
            Value = value;

        }

        // Private members

        public PatternMatch() {

            Success = false;

        }

    }

}
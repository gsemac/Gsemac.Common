using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public class CaseConverter :
        ICaseConverter {

        // Public members

        public CaseConverter(StringCase casing) :
            this(casing, CasingOptions.Default) {
        }
        public CaseConverter(StringCase casing, CasingOptions options) {

            this.casing = casing;
            this.options = options;

        }

        public string Convert(string input) {

            return ToCase(input, casing, options);

        }

        public static string ToCase(string input, StringCase casing) {

            return ToCase(input, casing, CasingOptions.Default);

        }
        public static string ToCase(string input, StringCase casing, CasingOptions options) {

            if (string.IsNullOrEmpty(input))
                return input;

            switch (casing) {

                case StringCase.Unchanged:
                    return input;

                case StringCase.Lower:
                    return input.ToLowerInvariant();

                case StringCase.Upper:
                    return input.ToUpperInvariant();

                case StringCase.Proper:
                    return ToProper(input, options);

                case StringCase.Sentence:
                    return ToSentence(input, options);

                default:
                    throw new ArgumentOutOfRangeException(nameof(input));

            }

        }

        public static string ToLower(string input) {

            if (string.IsNullOrEmpty(input))
                return input;

            return input.ToLowerInvariant();

        }

        public static string ToProper(string input) {

            return ToProper(input, CasingOptions.Default);

        }
        public static string ToProper(string input, CasingOptions options) {

            if (string.IsNullOrEmpty(input))
                return input;

            // TextInfo.ToTitleCase preserves sequences of all-caps, assuming that the sequence represents an acronym.
            // If we do not wish to preserve acronyms, we'll make the entire string lowercase first.

            if (!options.HasFlag(CasingOptions.PreserveAcronyms))
                input = input.ToLowerInvariant();

            string result = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input);

            // Fix possessive S (e.g. "'s") because TextInfo.ToTitleCase will capitalize them (e.g. "John's" -> "John'S").

            result = Regex.Replace(result, @"\b(['’])S\b", "$1s");

            if (options.HasFlag(CasingOptions.CapitalizeRomanNumerals))
                result = CapitalizeRomanNumerals(result);

            return result;

        }

        public static string ToSentence(string input) {

            return ToSentence(input, CasingOptions.Default, SentenceCaseOptions.Default);

        }
        public static string ToSentence(string input, CasingOptions options) {

            return ToSentence(input, options, SentenceCaseOptions.Default);

        }
        public static string ToSentence(string input, SentenceCaseOptions options) {

            return ToSentence(input, CasingOptions.Default, options);

        }
        public static string ToSentence(string input, CasingOptions options, SentenceCaseOptions sentenceCasingOptions) {

            if (string.IsNullOrEmpty(input))
                return input;

            if (!options.HasFlag(CasingOptions.PreserveAcronyms))
                input = input.ToLowerInvariant();

            string result = input;

            if (sentenceCasingOptions.HasFlag(SentenceCaseOptions.DetectMultipleSentences)) {

                // Detect multiple sentences in the same string, and convert them all to sentence case.

                string pattern = sentenceCasingOptions.HasFlag(SentenceCaseOptions.RequireWhitespaceAfterPunctuation) ?
                    @"(?:^|[\.!?]\s)\s*(\w)" :
                    @"(?:^|[\.!?])\s*(\w)";

                result = Regex.Replace(input, pattern, m => m.Value.ToUpperInvariant());

            }
            else {

                // Treat the entire string as a single sentence (only capitalize the first letter).

                result = Regex.Replace(input, @"^\s*(\w)", m => m.Value.ToUpperInvariant());

            }

            if (options.HasFlag(CasingOptions.CapitalizeRomanNumerals))
                result = CapitalizeRomanNumerals(result);

            return result;

        }

        public static string ToUpper(string input) {

            if (string.IsNullOrEmpty(input))
                return input;

            return input.ToUpperInvariant();

        }

        public static StringCase DetectCase(string input) {

            if (string.IsNullOrWhiteSpace(input))
                return StringCase.Unchanged;

            if (input.Equals(input.ToUpperInvariant()))
                return StringCase.Upper;

            if (input.Equals(input.ToLowerInvariant()))
                return StringCase.Lower;

            char[] firstCharsOfEachWord = Regex.Matches(input, @"\s*\b(.).+?\b").Cast<Match>().Select(m => m.Groups[1].Value.First()).ToArray();

            // If the first letter of each word is uppercase, assume we have Proper Case.

            if (firstCharsOfEachWord.All(c => !char.IsLower(c)))
                return StringCase.Proper;

            // If the first letter of the first word is uppercase, assume we have Sentence Case.

            if (firstCharsOfEachWord.Any() && !char.IsLower(firstCharsOfEachWord.First()))
                return StringCase.Sentence;

            // We could not detect the case.

            return StringCase.Unchanged;

        }

        // Private members

        private readonly StringCase casing;
        private readonly CasingOptions options;

        private static string CapitalizeRomanNumerals(string input) {

            // Regex adapted from Regular Expressions Cookbook, 6.9. Roman Numerals, example "Modern Roman numerals, strict":
            // https://www.oreilly.com/library/view/regular-expressions-cookbook/9780596802837/ch06s09.html

            const string romanNumeralsPattern = @"\b(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})\b";

            return Regex.Replace(input, romanNumeralsPattern, m => m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

        }

    }

}
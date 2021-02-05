using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public class CaseConverter :
        ICaseConverter {

        // Public members

        public CaseConverter(StringCasing casing, CasingOptions options = CasingOptions.Default) {

            this.casing = casing;
            this.options = options;

        }

        public string Convert(string input) {

            return ToCase(input, casing, options);

        }

        public static string ToCase(string input, StringCasing casing, CasingOptions options = CasingOptions.Default) {

            if (string.IsNullOrEmpty(input))
                return input;

            switch (casing) {

                case StringCasing.Unchanged:
                    return input;

                case StringCasing.Lower:
                    return input.ToLowerInvariant();

                case StringCasing.Upper:
                    return input.ToUpperInvariant();

                case StringCasing.Proper:
                    return ToProperCase(input, options);

                case StringCasing.Sentence:
                    return ToSentenceCase(input, options);

                default:
                    throw new ArgumentOutOfRangeException(nameof(input));

            }

        }
        public static string ToProperCase(string input, CasingOptions options = CasingOptions.Default) {

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
        public static string ToSentenceCase(string input) {

            return ToSentenceCase(input, CasingOptions.Default, SentenceCasingOptions.Default);

        }
        public static string ToSentenceCase(string input, CasingOptions options) {

            return ToSentenceCase(input, options, SentenceCasingOptions.Default);

        }
        public static string ToSentenceCase(string input, SentenceCasingOptions options) {

            return ToSentenceCase(input, CasingOptions.Default, options);

        }
        public static string ToSentenceCase(string input, CasingOptions options, SentenceCasingOptions sentenceCasingOptions) {

            if (string.IsNullOrEmpty(input))
                return input;

            if (!options.HasFlag(CasingOptions.PreserveAcronyms))
                input = input.ToLowerInvariant();

            string result = input;

            if (sentenceCasingOptions.HasFlag(SentenceCasingOptions.DetectMultipleSentences)) {

                // Detect multiple sentences in the same string, and convert them all to sentence case.

                string pattern = sentenceCasingOptions.HasFlag(SentenceCasingOptions.RequireWhitespaceAfterPunctuation) ?
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

        public static StringCasing DetectCase(string input) {

            if (string.IsNullOrWhiteSpace(input))
                return StringCasing.Unchanged;

            if (input.Equals(input.ToUpperInvariant()))
                return StringCasing.Upper;

            if (input.Equals(input.ToLowerInvariant()))
                return StringCasing.Lower;

            char[] firstCharsOfEachWord = Regex.Matches(input, @"\s*\b(.).+?\b").Cast<Match>().Select(m => m.Groups[1].Value.First()).ToArray();

            // If the first letter of each word is uppercase, assume we have Proper Case.

            if (firstCharsOfEachWord.All(c => !char.IsLower(c)))
                return StringCasing.Proper;

            // If the first letter of the first word is uppercase, assume we have Sentence Case.

            if (firstCharsOfEachWord.Any() && !char.IsLower(firstCharsOfEachWord.First()))
                return StringCasing.Sentence;

            // We could not detect the case.

            return StringCasing.Unchanged;

        }

        // Private members

        private readonly StringCasing casing;
        private readonly CasingOptions options;

        private static string CapitalizeRomanNumerals(string input) {

            // Regex adapted from Regular Expressions Cookbook, 6.9. Roman Numerals, example "Modern Roman numerals, strict":
            // https://www.oreilly.com/library/view/regular-expressions-cookbook/9780596802837/ch06s09.html

            const string romanNumeralsPattern = @"\b(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})\b";

            return Regex.Replace(input, romanNumeralsPattern, m => m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

        }

    }

}
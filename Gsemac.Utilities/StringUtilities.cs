using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Utilities {

    public static class StringUtilities {

        public static string After(string input, string substring) {

            return After(input, substring, StringComparison.CurrentCulture);

        }
        public static string After(string input, string substring, StringComparison comparisonType) {

            int index = input.IndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }

        public static IEnumerable<string> SplitAfter(string input, params char[] delimiters) {

            string pattern = "([" + Regex.Escape(string.Join("", delimiters)) + "])";
            string[] items = Regex.Split(input, pattern);

            for (int i = 0; i < items.Count(); i += 2) {

                string item = items[i];

                if (i + 1 < items.Count())
                    item += items[i + 1];

                yield return item;

            }

        }

        public static string NormalizeSpace(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            string pattern = options.HasFlag(NormalizeSpaceOptions.PreserveLineBreaks) ?
                @"[^\S\r\n]" :
                @"\s+";

            return Regex.Replace(input, pattern, " ");

        }

        public static string ToProperCase(string input, ProperCaseOptions options = ProperCaseOptions.None) {

            if (string.IsNullOrEmpty(input))
                return input;

            // TextInfo.ToTitleCase preserves sequences of all-caps, assuming that the sequence represents an acronym.
            // If we do not wish to preserve acronyms, we'll make the entire string lowercase first.

            if (!options.HasFlag(ProperCaseOptions.PreserveAcronyms))
                input = input.ToLowerInvariant();

            string result = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input);

            // Fix possessive S (e.g. "'s") because TextInfo.ToTitleCase will capitalize them (e.g. "John's" -> "John'S").

            result = Regex.Replace(result, @"\b(['’])S\b", "$1s");

            // Capitalize Roman numerals.

            if (options.HasFlag(ProperCaseOptions.CapitalizeRomanNumerals)) {

                // Regex adapted from Regular Expressions Cookbook, 6.9. Roman Numerals, example "Modern Roman numerals, strict":
                // https://www.oreilly.com/library/view/regular-expressions-cookbook/9780596802837/ch06s09.html

                const string romanNumeralsPattern = @"\b(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})\b";

                result = Regex.Replace(result, romanNumeralsPattern, m => m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

            }

            return result;

        }

    }

}
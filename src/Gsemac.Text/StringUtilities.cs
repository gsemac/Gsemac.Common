﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public static class StringUtilities {

        // Public members

        public static string After(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            int index = input.IndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }
        public static string AfterLast(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            int index = input.LastIndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }
        public static string Before(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            int index = input.IndexOf(substring, comparisonType);

            if (index >= 0)
                return input.Substring(0, index);
            else
                return input;

        }
        public static string BeforeLast(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            int index = input.LastIndexOf(substring, comparisonType);

            if (index >= 0)
                return input.Substring(0, index);
            else
                return input;

        }
        public static string Between(string input, string leftSubstring, string rightSubstring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            // Find start and end indices of the desired substring.

            int startIndex = input.IndexOf(leftSubstring, comparisonType);
            int endIndex = -1;

            if (startIndex >= 0) {

                startIndex += leftSubstring.Length;

                if (startIndex < input.Length)
                    endIndex = input.IndexOf(rightSubstring, startIndex, comparisonType);

            }

            // Get substring.

            if (startIndex >= 0 && endIndex >= 0)
                return input.Substring(startIndex, endIndex - startIndex);

            return string.Empty;

        }
        public static string BetweenLast(string input, string leftSubstring, string rightSubstring) {

            leftSubstring = Regex.Escape(leftSubstring);
            rightSubstring = Regex.Escape(rightSubstring);

            Regex regex = new Regex(string.Format("{0}(.*?){1}", leftSubstring, rightSubstring), RegexOptions.RightToLeft | RegexOptions.Singleline);

            Match match = regex.Match(input);

            if (match.Success)
                return match.Groups[1].Value;
            else
                return string.Empty;

        }
        public static IEnumerable<string> BetweenMany(string input, string leftSubstring, string rightSubstring) {

            return Regex.Matches(input, Regex.Escape(leftSubstring) + "(.+?)" + Regex.Escape(rightSubstring), RegexOptions.Singleline)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);

        }
        public static int Count(string input, string substring) {

            return (input.Length - input.Replace(substring, string.Empty).Length) / substring.Length;

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
        public static string ReplaceLast(string input, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {

            int index = input.LastIndexOf(oldValue, comparisonType);

            if (index >= 0)
                input = input.Remove(index, oldValue.Length).Insert(index, newValue);

            return input;

        }

        public static string NormalizeSpace(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            string pattern = options.HasFlag(NormalizeSpaceOptions.PreserveLineBreaks) ?
                @"[^\S\r\n]+" :
                @"\s+";

            return Regex.Replace(input, pattern, " ");

        }
        public static string NormalizeLineBreaks(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            string result = Regex.Replace(input, @"\r\n|\n", "\n");

            // Runs of 2 or more line breaks will be collapsed into a single line break unless the PreserveLineBreaks flag is set.

            if (!options.HasFlag(NormalizeSpaceOptions.PreserveLineBreaks)) {

                if (options.HasFlag(NormalizeSpaceOptions.PreserveParagraphBreaks))
                    result = Regex.Replace(result, @"\n{3,}", "\n\n");
                else
                    result = Regex.Replace(result, @"\n+", "\n");

            }

            // Replace the temporary newlines with the evironment's newline sequence.

            result = Regex.Replace(result, @"\n", Environment.NewLine);

            return result;

        }

        public static string Unescape(string input, UnescapeOptions options = UnescapeOptions.Default) {

            if (string.IsNullOrEmpty(input))
                return input;

            string result = input;

            if (options.HasFlag(UnescapeOptions.RepairTextEncoding))
                result = RepairTextEncoding(result);

            // Build the regex that will match different kinds of escape sequences.

            List<string> escapePatterns = new List<string>();

            if (options.HasFlag(UnescapeOptions.UnescapeHtmlEntities)) {

                // The following regex pattern is sourced from https://stackoverflow.com/a/56490838/5383169 (mahoor13)

                escapePatterns.Add(@"&([a-z0-9]+|#[0-9]{1,6}|#x[0-9a-f]{1,6});");

            }

            if (options.HasFlag(UnescapeOptions.UnescapeUriEncoding))
                escapePatterns.Add(@"%(?:u[0-9a-f]{4}|[0-9a-f]{2})");

            if (options.HasFlag(UnescapeOptions.UnescapeEscapeSequences))
                escapePatterns.Add(@"\\(?:u[0-9a-f]{4}|x[0-9a-f]{2,4}|.)");

            // Replace all escape sequences.

            Regex escapeRegex = new Regex(string.Join("|", escapePatterns.Select(pattern => $"(?:{pattern})")), RegexOptions.IgnoreCase | RegexOptions.Singleline);

            result = escapeRegex.Replace(result, m => UnescapeEscapeSequence(m.Value));

            return result;

        }

        public static string ToCase(string input, StringCasing casing) {

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
                    return ToProperCase(input);

                case StringCasing.Sentence:
                    return ToSentenceCase(input);

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

        public static bool IsNumeric(string input, NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint) {

            return double.TryParse(input, style, CultureInfo.InvariantCulture, out _);

        }
        public static string PadLeadingDigits(string input, int numberOfDigits) {

            // Trim all existing leading zeros.

            input = (input ?? "").TrimStart('0');

            // Make sure the string contains at least one (whole) digit.

            if (input.Length <= 0 || input.StartsWith("."))
                input = "0" + input;

            // Pad the string with zeros so that the leading digits have a length of /at least/ the desired number of digits.
            // If there are already more leading digits than desired, no padding is added.

            int currentLeadingDigits = input.IndexOf(".");

            if (currentLeadingDigits < 0)
                currentLeadingDigits = input.Length;

            int paddingLength = Math.Max(numberOfDigits - currentLeadingDigits, 0);

            input = "".PadLeft(paddingLength, '0') + input;

            return input;

        }

        public static StringComparer GetStringComparer(StringComparison stringComparison) {

            Dictionary<StringComparison, StringComparer> dict = new Dictionary<StringComparison, StringComparer> {
                { StringComparison.CurrentCulture, StringComparer.CurrentCulture },
                { StringComparison.CurrentCultureIgnoreCase, StringComparer.CurrentCultureIgnoreCase },
                { StringComparison.InvariantCulture, StringComparer.InvariantCulture },
                { StringComparison.InvariantCultureIgnoreCase, StringComparer.InvariantCultureIgnoreCase },
                { StringComparison.Ordinal, StringComparer.Ordinal },
                { StringComparison.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase }
            };

            if (dict.TryGetValue(stringComparison, out StringComparer stringComparer))
                return stringComparer;

            throw new ArgumentOutOfRangeException(nameof(stringComparison));

        }

        // Private members

        private static string UnescapeEscapeSequence(string input) {

            if (input.StartsWith("&")) {

                // Unescape HTML entities (e.g. "&#038;" -> "&").
                // Note that HtmlDecode is CASE-SENSITIVE, although web browsers generally handle HTML entities in a case-insensitive manner.

                return System.Net.WebUtility.HtmlDecode(input.ToLowerInvariant());

            }
            else if (input.Length == 2) {

                switch (input.Substring(1)) {

                    case "0":
                        return "\0";

                    case "a":
                        return "\a";

                    case "b":
                        return "\b";

                    case "f":
                        return "\f";

                    case "n":
                        return "\n";

                    case "r":
                        return "\r";

                    case "t":
                        return "\t";

                    case "v":
                        return "\v";

                    default:
                        return input.Substring(1);

                }

            }
            else {

                bool octal = false;

                if (input.StartsWith("\\")) {

                    // Denotes escape sequences (e.g., "\u2320" -> "⌠", "\n" = newline, etc.).

                    input = input.Substring(1);

                    if (input.All(c => char.IsDigit(c) && c >= '0' && c < '8'))
                        octal = true;
                    else
                        input = input.Substring(1); // skip "u" or "x"

                }
                else if (input.StartsWith("%")) {

                    // Denotes data URI encoding (e.g. "%20" -> " ").

                    // Notes on Uri.UnescapeDataString (and why it's not used here):
                    // - Ignores unicode escape sequences (e.g. "%u0107" -> "ć")
                    // - Can throw an exception on Windows XP if there are any percent symbols that aren't part of an escape sequence (?).

                    input = input.TrimStart('%', 'u');

                }

                if (octal) {

                    // Parse number as octal.

                    return char.ConvertFromUtf32(Convert.ToInt32(input, 8)).ToString();

                }
                else {

                    // Parse number as hex.

                    if (int.TryParse(input, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedInt))
                        return char.ConvertFromUtf32(parsedInt).ToString();

                }

            }

            // If we get here, the escape sequence wasn't recognized.

            return input;

        }
        private static string RepairTextEncoding(string input) {

            StringBuilder sb = new StringBuilder(input);

            // Fix miscellaneous encoding issues.
            // These cases are based on instances found "in the wild", and are not guaranteed to be perfect because we cannot be sure what the original encoding was.

            sb.Replace(@"â€™", @"'");
            sb.Replace(@"â˜†", @"☆");

            return sb.ToString();

        }

        private static string CapitalizeRomanNumerals(string input) {

            // Regex adapted from Regular Expressions Cookbook, 6.9. Roman Numerals, example "Modern Roman numerals, strict":
            // https://www.oreilly.com/library/view/regular-expressions-cookbook/9780596802837/ch06s09.html

            const string romanNumeralsPattern = @"\b(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})\b";

            return Regex.Replace(input, romanNumeralsPattern, m => m.Value.ToUpperInvariant(), RegexOptions.IgnoreCase);

        }

    }

}
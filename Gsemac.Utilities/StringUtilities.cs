﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Utilities {

    public static class StringUtilities {

        // Public members

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
        public static string Unescape(string input) {

            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder sb = new StringBuilder(input);

            // Fix miscellaneous encoding issues.
            // These cases are based on instances found "in the wild", and are not guaranteed to be perfect because we cannot be sure what the original encoding was.

            sb.Replace(@"â€™", @"'");
            sb.Replace(@"â˜†", @"☆");

            // Unescape backslash-escaped quotes (from JSON strings, for example).

            sb.Replace(@"\'", @"'");
            sb.Replace(@"\""", @"""");

            input = sb.ToString();

            input = UnescapeHtmlEntities(input);
            input = UnescapeDataString(input);
            input = UnescapeUnicodeEscapeSequences(input);

            return input;

        }

        public static string ToProperCase(string input, ProperCaseOptions options = ProperCaseOptions.Default) {

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

        // Private members

        private static string UnescapeHtmlEntities(string input) {

            // Unescape HTML entities (e.g. "&#038;" -> "&").
            // Note that HtmlDecode on its own is NOT case-insensitive, although web browsers generally handle HTML entities in a case-insensitive manner.

            // The following regex pattern is sourced from https://stackoverflow.com/a/56490838/5383169 (mahoor13)

            string htmlEntityPattern = "&([a-z0-9]+|#[0-9]{1,6}|#x[0-9a-f]{1,6});";

            input = Regex.Replace(input, htmlEntityPattern, m => m.Value.ToLowerInvariant(), RegexOptions.IgnoreCase);

            input = System.Net.WebUtility.HtmlDecode(input);

            return input;

        }
        private static string UnescapeDataString(string input) {

            // Unescape data string encoding (e.g. "%20" -> " ").
            // UnescapeDataString can throw an exception on Windows XP if there are any percent symbols that aren't part of an escape sequence (?).

#pragma warning disable CA1031 // Do not catch general exception types
            try {

                input = Uri.UnescapeDataString(input);

            }
            catch (Exception) { }
#pragma warning restore CA1031 // Do not catch general exception types

            return input;

        }
        private static string UnescapeUnicodeEscapeSequences(string input) {

            // Unescape unicode escape sequences (e.g., "\u2320" -> "⌠").

            string unicodeEscapeSequencePattern = @"\\u([0-9a-f]{4})";

            input = Regex.Replace(input, unicodeEscapeSequencePattern, m => ParseUnicodeEscapeSequence(m.Value), RegexOptions.IgnoreCase);

            return input;

        }
        private static string ParseUnicodeEscapeSequence(string input) {

            if (int.TryParse(input.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedInt))
                return char.ConvertFromUtf32(parsedInt).ToString();

            return input;

        }

    }

}
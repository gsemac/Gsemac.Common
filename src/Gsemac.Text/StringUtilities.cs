using Gsemac.Text.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text {

    public static class StringUtilities {

        // Public members

        public static string After(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            int index = input.IndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }
        public static string AfterLast(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            int index = input.LastIndexOf(substring, comparisonType);

            if (index >= 0) {

                index += substring.Length;

                return input.Substring(index, input.Length - index);

            }
            else
                return input;

        }
        public static string Before(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            int index = -1;

            if (!string.IsNullOrEmpty(substring))
                index = input.IndexOf(substring, comparisonType);

            if (index >= 0)
                return input.Substring(0, index);
            else
                return input;

        }
        public static string BeforeLast(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            int index = input.LastIndexOf(substring, comparisonType);

            if (index >= 0)
                return input.Substring(0, index);
            else
                return input;

        }
        public static string Between(string input, string leftSubstring, string rightSubstring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(leftSubstring) || string.IsNullOrEmpty(rightSubstring))
                return input;

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
        public static string BetweenLast(string input, string leftSubstring, string rightSubstring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(leftSubstring) || string.IsNullOrEmpty(rightSubstring))
                return input;

            leftSubstring = Regex.Escape(leftSubstring);
            rightSubstring = Regex.Escape(rightSubstring);

            RegexOptions regexOptions = RegexOptions.RightToLeft | RegexOptions.Singleline;

            if (comparisonType == StringComparison.InvariantCultureIgnoreCase || comparisonType == StringComparison.Ordinal || comparisonType == StringComparison.OrdinalIgnoreCase)
                regexOptions |= RegexOptions.CultureInvariant;

            if (comparisonType == StringComparison.CurrentCultureIgnoreCase || comparisonType == StringComparison.InvariantCultureIgnoreCase || comparisonType == StringComparison.InvariantCultureIgnoreCase)
                regexOptions |= RegexOptions.IgnoreCase;

            Regex regex = new Regex(string.Format("{0}(.*?){1}", leftSubstring, rightSubstring), regexOptions);

            Match match = regex.Match(input);

            if (match.Success)
                return match.Groups[1].Value;
            else
                return string.Empty;

        }
        public static IEnumerable<string> BetweenMany(string input, string leftSubstring, string rightSubstring) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(leftSubstring) || string.IsNullOrEmpty(rightSubstring))
                return Enumerable.Empty<string>();

            return Regex.Matches(input, Regex.Escape(leftSubstring) + "(.+?)" + Regex.Escape(rightSubstring), RegexOptions.Singleline)
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);

        }
        public static int Count(string input, string substring) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return 0;

            return (input.Length - input.Replace(substring, string.Empty).Length) / substring.Length;

        }

        public static string Reverse(string input) {

            if (string.IsNullOrEmpty(input))
                return input;

            // The following solution is based on the one given here: https://stackoverflow.com/a/15029493/5383169 (Michael Liu)
            // The naive solution of reversing the result of ToCharArray is tempting but invalid, as it doesn't work correctly with multi-byte characters (e.g. "אֳ").
            // See here for more details: https://stackoverflow.com/q/15029238/5383169

            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(input);

            List<string> elements = new List<string>(input.Length);

            while (enumerator.MoveNext())
                elements.Add(enumerator.GetTextElement());

            elements.Reverse();

            return string.Concat(elements);

        }

        public static IEnumerable<string> Split(string value, char separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, new[] { separator }, count, options);

        }
        public static IEnumerable<string> Split(string value, string[] separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            if (string.IsNullOrEmpty(value))
                yield break;

            IEnumerable<string> items;

            if (options <= StringSplitOptions.TrimEntries) {

                // No special options have been specified, so we can do a normal string split.

                items = value.Split(separator, count, GetStringSplitOptions(options));

            }
            else {

                items = SplitWithDelimiters(value, separator, count, options);

            }

            foreach (string item in ApplyPostSplitOptions(items, options))
                yield return item;

        }
        public static IEnumerable<string> Split(string value, char[] separators, int count, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, separators is null ? null : separators.Select(c => c.ToString()).ToArray(), count, options);

        }
        public static IEnumerable<string> Split(string value, string[] separators, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, separators, int.MaxValue, options);

        }
        public static IEnumerable<string> Split(string value, string separator, int count, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, new[] { separator }, count, options);

        }
        public static IEnumerable<string> Split(string value, char[] separators, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, separators is null ? null : separators.Select(c => c.ToString()).ToArray(), int.MaxValue, options);

        }
        public static IEnumerable<string> Split(string value, char separator, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, separator, int.MaxValue, options);

        }
        public static IEnumerable<string> Split(string value, string separator, StringSplitOptions options = StringSplitOptions.None) {

            return Split(value, separator, int.MaxValue, options);

        }

        public static string ReplaceFirst(string input, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {

            // Note that "oldValue" and "newValue" match the parameter names of the vanilla Replace method.

            if (string.IsNullOrEmpty(oldValue) || oldValue.Length <= 0)
                throw new ArgumentException(ExceptionMessages.StringCannotBeOfZeroLength, nameof(oldValue));

            if (string.IsNullOrEmpty(input))
                return input;

            int index = input.IndexOf(oldValue, comparisonType);

            if (index >= 0)
                input = input.Remove(index, oldValue.Length).Insert(index, newValue ?? "");

            return input;

        }
        public static string ReplaceLast(string input, string oldValue, string newValue, StringComparison comparisonType = StringComparison.CurrentCulture) {

            // Note that "oldValue" and "newValue" match the parameter names of the vanilla Replace method.

            if (string.IsNullOrEmpty(oldValue) || oldValue.Length <= 0)
                throw new ArgumentException(ExceptionMessages.StringCannotBeOfZeroLength, nameof(oldValue));

            if (string.IsNullOrEmpty(input))
                return input;

            int index = input.LastIndexOf(oldValue, comparisonType);

            if (index >= 0)
                input = input.Remove(index, oldValue.Length).Insert(index, newValue ?? "");

            return input;

        }

        public static string NormalizeWhiteSpace(string input, string replacement, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            if (string.IsNullOrEmpty(input))
                return input;

            string pattern = options.HasFlag(NormalizeSpaceOptions.PreserveLineBreaks) ?
                @"[^\S\r\n]+" :
                @"\s+";

            string result = Regex.Replace(input, pattern, replacement);

            if (options.HasFlag(NormalizeSpaceOptions.Trim))
                result = result.Trim();

            return result;

        }
        public static string NormalizeWhiteSpace(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            return NormalizeWhiteSpace(input, " ", options);

        }
        public static string NormalizeLineBreaks(string input, NormalizeSpaceOptions options = NormalizeSpaceOptions.Default) {

            if (string.IsNullOrEmpty(input))
                return input;

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

            if (options.HasFlag(NormalizeSpaceOptions.Trim))
                result = result.Trim();

            return result;

        }

        public static string TrimStart(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            if (input.StartsWith(substring, comparisonType))
                return TrimStart(input.Substring(substring.Length), substring);

            return input;

        }
        public static string TrimEnd(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return input;

            if (input.EndsWith(substring, comparisonType))
                return TrimEnd(input.Substring(0, input.Length - substring.Length), substring);

            return input;

        }
        public static string Trim(string input, string substring, StringComparison comparisonType = StringComparison.CurrentCulture) {

            return TrimEnd(TrimStart(input, substring, comparisonType), substring, comparisonType);

        }
        public static string TrimOrDefault(string input) {

            if (string.IsNullOrEmpty(input))
                return input;

            return input.Trim();

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

        public static bool IsNumeric(string input) {

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // The idea behind this method is that it should return true for any string a naive observer would consider to be a number.
            // By default, decimals and negative numbers are considered numeric.

            // Allow a leading a sign for negative numbers, but not positive numbers.

            if (input.TrimStart().StartsWith("+"))
                return false;

            return IsNumeric(input, NumberStyles.Integer | NumberStyles.AllowDecimalPoint);

        }
        public static bool IsNumeric(string input, NumberStyles styles) {

            if (string.IsNullOrWhiteSpace(input))
                return false;

            return double.TryParse(input, styles, CultureInfo.InvariantCulture, out _);

        }
        public static string PadDigits(string input, int numberOfDigits) {

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

        public static int ComputeLevenshteinDistance(string str1, string str2) {

            // This algorithm was adapted from the one given here: https://stackoverflow.com/a/57961456 (ilike2breakthngs)
            // It is a C# port of the Java inplementation given here: http://web.archive.org/web/20120526085419/http://www.merriampark.com/ldjava.htm

            str1 = str1 ?? "";
            str2 = str2 ?? "";

            int n = str1.Length;
            int m = str2.Length;

            if (n == 0)
                return m;

            if (m == 0)
                return n;

            int[] p = new int[n + 1]; //'previous' cost array, horizontally
            int[] d = new int[n + 1]; // cost array, horizontally

            int i; // iterates through s
            int j; // iterates through t

            for (i = 0; i <= n; i++) {
                p[i] = i;
            }

            for (j = 1; j <= m; j++) {

                char tJ = str2[j - 1]; // jth character of t

                d[0] = j;

                for (i = 1; i <= n; i++) {

                    int cost = str1[i - 1] == tJ ? 0 : 1; // cost

                    // minimum of cell to the left+1, to the top+1, diagonally left and up +cost

                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);

                }

                // copy current distance counts to 'previous row' distance counts

                int[] dPlaceholder = p; // placeholder to assist in swapping p and d

                p = d;
                d = dPlaceholder;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts

            return p[n];

        }
        public static double ComputeSimilarity(string str1, string str2) {

            if (str1.Equals(str2))
                return 1.0;

            int levenshteinDistance = ComputeLevenshteinDistance(str1, str2);
            int maxLength = Math.Max(str1.Length, str2.Length);

            return 1.0 - (levenshteinDistance / maxLength);

        }

        public static string ComputeMD5Hash(string input, Encoding encoding = null) {

            if (input is null)
                throw new ArgumentNullException(nameof(input));

            encoding = encoding ?? Encoding.UTF8;

            using (MD5 md5 = MD5.Create())
            using (Stream stream = new MemoryStream(encoding.GetBytes(input))) {

                var hash = md5.ComputeHash(stream);

                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            }

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

            // The "â€" sequence seems to be very common in garbled texts (mojibake).
            // They are most likely the result of decoding UTF8 as windows-1252.

            if (input.Contains("â€"))
                input = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(input));

            // These extra cases are derived from instances found "in the wild".

            StringBuilder sb = new StringBuilder(input);

            sb.Replace(@"â€˜", @"‘");
            sb.Replace(@"â€™", @"’");
            sb.Replace(@"â€”", @"—");
            sb.Replace(@"â€“", @"–");
            sb.Replace(@"â˜†", @"☆");
            sb.Replace(@"â€¢", @"-");
            sb.Replace(@"â€¦", @"…");
            sb.Replace(@"â€œ", @"“");
            sb.Replace(@"â€", @"”");

            return sb.ToString();

        }

        private static System.StringSplitOptions GetStringSplitOptions(StringSplitOptions options) {

            System.StringSplitOptions result = System.StringSplitOptions.None;

            if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
                result |= System.StringSplitOptions.RemoveEmptyEntries;

            return result;

        }
        private static IEnumerable<string> ApplyPostSplitOptions(IEnumerable<string> items, StringSplitOptions options) {

            IEnumerator<string> enumerator = items.GetEnumerator();
            bool onFirstItem = true;

            while (enumerator.MoveNext()) {

                string item = enumerator.Current;

                if (options.HasFlag(StringSplitOptions.TrimEntries) && !string.IsNullOrEmpty(item))
                    item = item.Trim();

                bool hasNextItem = ((!onFirstItem && options.HasFlag(StringSplitOptions.PrependDelimiter)) ||
                    options.HasFlag(StringSplitOptions.AppendDelimiter)) &&
                    enumerator.MoveNext();

                if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries) || !string.IsNullOrEmpty(item)) {

                    if (hasNextItem) {

                        string nextItem = enumerator.Current;

                        item += nextItem;

                    }

                    yield return item;

                    onFirstItem = false;

                }

            }

        }
        private static IEnumerable<string> SplitWithDelimiters(string value, string[] separators, int count, StringSplitOptions options) {

            int startIndex = 0;
            int itemCount = 0;

            for (int i = 0; i < value.Length && itemCount < count; ++i) {

                if (options.HasFlag(StringSplitOptions.RespectEnclosingPunctuation)) {

                    char currentChar = value[i];

                    if (CharUtilities.IsLeftEnclosingPunctuation(currentChar)) {

                        // We found a left enclosing character, so move forward to its partner before checking for delimiters.

                        int partnerIndex = TryFindRightEnclosingPunctuationIndex(value, i + 1, currentChar);

                        if (partnerIndex >= 0)
                            i = partnerIndex + 1;

                    }

                }

                // Find the end of any of the delimiters.
                // If the list of delimiters is null, we'll split on any whitespace (same as .NET's implementation).

                int endIndex = separators is null ?
                    (char.IsWhiteSpace(value[i]) ? i : -1) :
                    TryFindSeparatorEndIndex(value, i, separators);

                if (endIndex >= 0) {

                    yield return value.Substring(startIndex, i - startIndex);

                    if (options.HasFlag(StringSplitOptions.PrependDelimiter) || options.HasFlag(StringSplitOptions.AppendDelimiter))
                        yield return value.Substring(i, endIndex - i + 1);

                    i = endIndex;
                    startIndex = i + 1;

                    ++itemCount;

                }

            }

            // Return the last item (remainder of the string).

            yield return value.Substring(startIndex, value.Length - startIndex);

        }
        private static int TryFindSeparatorEndIndex(string value, int startIndex, string[] separators) {

            foreach (string separator in separators) {

                int i = startIndex;
                int j = 0;

                while (i < value.Length && j < separator.Length) {

                    if (value[i] != separator[j])
                        break;

                    ++i;
                    ++j;

                }

                if (j >= separator.Length)
                    return startIndex + j - 1;

            }

            return -1;

        }
        private static int TryFindRightEnclosingPunctuationIndex(string value, int startIndex, char leftEnclosingPunctuation) {

            char rightEnclosingPunctuation = CharUtilities.GetOppositeEnclosingPunctuation(leftEnclosingPunctuation);
            int openCount = 1;

            for (int i = startIndex; i < value.Length; ++i) {

                if (value[i] == rightEnclosingPunctuation)
                    --openCount;
                else if (value[i] == leftEnclosingPunctuation)
                    ++openCount;

                if (openCount <= 0)
                    return i;

            }

            return -1;


        }

    }

}
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

    }

}
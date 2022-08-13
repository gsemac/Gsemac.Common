using Gsemac.Reflection.Extensions;
using Gsemac.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript {

    public static class JSGlobal {

        // Public members

        public static string DecodeUri(string str) {

            return DecodeUriComponent(str);

        }
        public static string DecodeUriComponent(string str) {

            return Uri.UnescapeDataString(str);

        }

        public static string EncodeUri(string str) {

            return Uri.EscapeUriString(str);

        }
        public static string EncodeUriComponent(string str) {

            string result = Uri.EscapeDataString(str);

            // Characters -_.!~*'() should NOT be escaped, but EscapeDataString will escape them.

            string unescapedCharacterPattern = string.Join("|", "-_.!~*'()"
                .Select(c => Regex.Escape(Uri.EscapeDataString(c.ToString()))));

            result = Regex.Replace(result, unescapedCharacterPattern, m => Uri.UnescapeDataString(m.Value));

            return result;

        }

        public static bool IsNaN(object value) {

            if (value is null)
                return false;

            // Numeric types are always numbers

            if (value.GetType().IsNumeric())
                return false;

            if (value is string str) {

                // Empty string is converted to 0

                if (string.IsNullOrWhiteSpace(str))
                    return false;

                return !double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedNumber)
                    || double.IsNaN(parsedNumber);

            }

            // The input could not be interpreted as a number.

            return true;

        }

        public static int? ParseInt(string str) {

            int radix = 10;

            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {

                radix = 16;

            }
            else if (str.StartsWith("0")) {

                // To be ECMAScript 5-compliant, leading zeros should not cause the value to be treated as octal.
                // https://stackoverflow.com/questions/28863450/parseint-and-octal-which-browsers-support-it
                // This mimics how modern browsers, such as Google Chrome, would interpret the input.

            }

            return ParseInt(str, radix);

        }
        public static int? ParseInt(string str, int radix) {

            Match numberMatch = Regex.Match(str, @"(?:[-+]|0x)?\d+");

            if (!numberMatch.Success)
                return null;

            try {

                return Convert.ToInt32(numberMatch.Value, radix);

            }
            catch (ArgumentException) {

                return null;

            }

        }

        public static string Unescape(string str) {

            str = StringUtilities.Unescape(str, UnescapeOptions.UnescapeEscapeSequences | UnescapeOptions.UnescapeUriEncoding);

            return str;

        }

    }

}
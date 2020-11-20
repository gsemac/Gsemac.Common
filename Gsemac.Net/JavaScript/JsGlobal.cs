using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript {

    public static class JsGlobal {

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

            return Uri.EscapeDataString(str);

        }
        public static int? ParseInt(string str) {

            int radix = 10;

            if (str.StartsWith("0x")) {

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

            // Replace escaped characters ("\x", "\n", "\\", etc.).

            Regex escapedCharacterPattern = new Regex(@"\\(x[\da-f]{1,2}|[n\\])", RegexOptions.IgnoreCase);

            str = escapedCharacterPattern.Replace(str, m => {

                string match = m.Groups[1].Value;

                switch (match) {

                    case "n":
                        return "\n";

                    case "\\":
                        return match;

                    default:
                        return Convert.ToChar(Convert.ToUInt32(match.Substring(1), 16)).ToString();

                }

            });

            return str;

        }

    }

}
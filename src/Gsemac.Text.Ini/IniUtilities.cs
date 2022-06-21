using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public static class IniUtilities {

        // Public members

        public static string Escape(string input) {

            string result = Regex.Replace(input, @"[\\'""\x00\a\b\t\r\n;#=:]",
                m => {

                    switch (m.Value) {

                        case "\0":
                            return @"\0";

                        case "\a":
                            return @"\a";

                        case "\b":
                            return @"\b";

                        case "\t":
                            return @"\t";

                        case "\r":
                            return @"\r";

                        case "\n":
                            return @"\n";

                        default:
                            return $@"\{m.Value}";

                    }

                }, RegexOptions.IgnoreCase);

            return result;

        }
        public static string Unescape(string input) {

            return StringUtilities.Unescape(input, UnescapeOptions.UnescapeEscapeSequences);

        }

    }

}
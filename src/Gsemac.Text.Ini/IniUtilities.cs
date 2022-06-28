using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public static class IniUtilities {

        // Public members

        public static string Escape(string input) {

            return Escape(input, IniOptions.Default);

        }
        public static string Escape(string input, IIniOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(input))
                return input;

            // Add custom delimiters to the pattern.

            string pattern = string.Join("|", new[] {
                @"[\\'""\x00\a\b\t\r\n\[\]]"
            }
            .Concat(new[] {
                options.CommentMarker,
                options.NameValueSeparator,
            }
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => Regex.Escape(p))));

            string result = Regex.Replace(input, pattern,
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
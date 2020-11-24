using Gsemac.Core;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Ini {

    public class IniFile {

        // Public members

        public static string Escape(string input) {

            string result = Regex.Replace(input, @"[\\'""\x00\a\b\t\r\n;#=:]",
                m => $@"\{m.Value}", RegexOptions.IgnoreCase);

            return result;

        }
        public static string Unescape(string input) {

            string result = Regex.Replace(input, @"\\(?:x[0-9a-z]{2,4}|.)",
                m => StringUtilities.Unescape(m.Value, UnescapeOptions.UnescapeEscapeSequences), RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return result;

        }

    }

}
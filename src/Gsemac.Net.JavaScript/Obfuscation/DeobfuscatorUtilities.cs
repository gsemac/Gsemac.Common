using System.Globalization;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    internal static class DeobfuscatorUtilities {

        // Public members

        public static bool TryUnescapeString(string value, out string result) {

            result = value;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            bool success = true;

            result = Regex.Replace(value, @"\\x(?<charcode>[a-f0-9]{2})", m => {

                if (int.TryParse(m.Groups["charcode"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int charCode)) {

                    return JSString.FromCharCode(charCode);

                }
                else {

                    success = false;

                    return m.Value;
                }

            });

            return success;

        }

    }

}
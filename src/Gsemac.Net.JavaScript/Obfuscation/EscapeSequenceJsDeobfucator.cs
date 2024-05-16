using System.Globalization;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public sealed class EscapeSequenceJsDeobfucator :
        IJSDeobfuscator {

        // Public members

        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            if (string.IsNullOrWhiteSpace(script))
                return false;

            bool anyReplaced = false;

            result = Regex.Replace(script, @"""(?:\\x[a-f0-9]{2})+""|'(?:\\x[a-f0-9]{2})+'", m => {

                anyReplaced = true;

                if (TryUnescapeString(m.Value, out string unescapedValue)) {

                    anyReplaced = true;

                    return unescapedValue;

                }
                else {

                    return m.Value;

                }

            });

            return anyReplaced;

        }

        // Private members

        private static bool TryUnescapeString(string value, out string result) {

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
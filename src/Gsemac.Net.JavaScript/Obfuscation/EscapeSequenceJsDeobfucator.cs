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

                if (DeobfuscatorUtilities.TryUnescapeString(m.Value, out string unescapedValue)) {

                    anyReplaced = true;

                    return unescapedValue;

                }
                else {

                    return m.Value;

                }

            });

            return anyReplaced;

        }

    }

}
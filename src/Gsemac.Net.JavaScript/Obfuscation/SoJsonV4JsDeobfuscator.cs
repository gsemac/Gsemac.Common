using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public sealed class SoJsonV4JsDeobfuscator :
        IJSDeobfuscator {

        // Public members
        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            if (string.IsNullOrWhiteSpace(script))
                return false;

            // This implementation is based off of the one in JSDec.
            // https://github.com/hax0r31337/JSDec/blob/master/js/dec.js#L24

            // The input will start with either "["sojson.v4"]" or "['sojson.v4']".

            Match match = Regex.Match(script,
                @"(?:\['sojson.v4'\]|\[""sojson.v4""\])\.filter\.constructor\(.+?apply\(null,[""'](?<payload>[^'""]+)[""']");

            if (!match.Success)
                return false;

            string payload = match.Groups["payload"].Value;

            if (string.IsNullOrWhiteSpace(payload))
                return false;

            string[] args = Regex.Split(payload, @"[a-zA-Z]{1,}");

            StringBuilder resultBuilder = new StringBuilder();

            foreach (string arg in args) {

                if (!int.TryParse(arg, out int charCode))
                    return false;

                resultBuilder.Append(JSString.FromCharCode(charCode));

            }

            result = resultBuilder.ToString();

            return true;

        }

    }

}
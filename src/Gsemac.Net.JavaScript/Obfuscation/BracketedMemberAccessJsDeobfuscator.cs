using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public sealed class BracketedMemberAccessJsDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            if (string.IsNullOrWhiteSpace(script))
                return false;

            bool anyReplaced = false;

            result = Regex.Replace(script, @"(?<=[\]\)'""a-zA-Z_])(?:\['[^']+'\]|\[""[^""]+""\])", m => {

                anyReplaced = true;

                string matchValue = m.Value;

                return $".{matchValue.Substring(2, matchValue.Length - 4)}";

            });

            return anyReplaced;

        }

    }

}
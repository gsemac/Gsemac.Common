using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public sealed class DeanEdwardsPackerJsDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            if (string.IsNullOrWhiteSpace(script))
                return false;

            bool anyReplaced = false;

            result = Regex.Replace(script, @"\beval\s*\(\s*(?<unpacker>function\s*\((?:\s*[a-z]{1}\s*\,?\s*){6}\s*\).+?{}\))\s*\)",
                m => {

                    anyReplaced = true;

                    return Unpack(m.Groups["unpacker"].Value);

                },
                RegexOptions.Singleline);

            return anyReplaced;

        }

        // Private members

        private static string Unpack(string unpackerScript) {

            if (string.IsNullOrWhiteSpace(unpackerScript))
                return unpackerScript;

            using (IJsEngine engine = new JintJsEngine())
                return engine.Evaluate($"({unpackerScript})").ToString();

        }

    }

}
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public class DeanEdwardsPackerDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public string Deobfuscate(string script) {

            string deobfuscated = Regex.Replace(script, @"\beval\s*\(\s*(?<unpacker>function\s*\((?:\s*[a-z]{1}\s*\,?\s*){6}\s*\).+?{}\))\s*\)",
                m => Unpack(m.Groups["unpacker"].Value),
                RegexOptions.Singleline);

            return deobfuscated;

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
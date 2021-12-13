using Jint;
using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public class DeanEdwardsPackerDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public string Deobfuscate(string script) {

            string deobfuscated = Regex.Replace(script, @"\beval\((?<unpacker>function\((?:[a-z]{1}\,?\s*){6}\).+?{}\))\)",
                m => Unpack(m.Groups["unpacker"].Value),
                RegexOptions.Singleline);

            return deobfuscated;

        }

        // Private members

        private static string Unpack(string unpackerScript) {

            Engine engine = new Engine();

            return engine.Execute($"({unpackerScript})")
                .GetCompletionValue()
                .AsString();

        }

    }

}
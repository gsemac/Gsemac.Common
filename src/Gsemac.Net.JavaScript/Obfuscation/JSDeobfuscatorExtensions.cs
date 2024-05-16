using System;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public static class JSDeobfuscatorExtensions {

        // Public members

        public static string Deobfuscate(this IJSDeobfuscator jsDeobfuscator, string script) {

            if (jsDeobfuscator is null)
                throw new ArgumentNullException(nameof(jsDeobfuscator));

            if (string.IsNullOrEmpty(script))
                return script;

            if (jsDeobfuscator.TryDeobfuscate(script, out string result))
                return result;

            return script;

        }

    }

}
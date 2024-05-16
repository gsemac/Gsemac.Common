using System;
using System.Collections.Generic;

namespace Gsemac.Net.JavaScript.Obfuscation {

    internal sealed class CompositeJsDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public CompositeJsDeobfuscator(IJSDeobfuscator[] jsDeobfucators) {

            if (jsDeobfucators is null)
                throw new ArgumentNullException(nameof(jsDeobfucators));

            this.jsDeobfucators = jsDeobfucators;

        }

        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            bool success = false;

            // We will apply deobfuscation repeatedly until the result stops changing/we encounter a cycle.

            HashSet<int> seenStates = new HashSet<int> {
                result.GetHashCode()
            };

            while (true) {

                foreach (IJSDeobfuscator deobfuscator in jsDeobfucators)
                    deobfuscator.TryDeobfuscate(result, out result);

                int resultHashCode = result.GetHashCode();

                if (seenStates.Contains(resultHashCode))
                    break;

                success = true;

                seenStates.Add(resultHashCode);

            }

            return success;

        }

        // Private members

        private readonly IJSDeobfuscator[] jsDeobfucators;

    }

}
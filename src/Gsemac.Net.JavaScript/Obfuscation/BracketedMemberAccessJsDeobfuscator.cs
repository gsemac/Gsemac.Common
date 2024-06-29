using System.Text.RegularExpressions;

namespace Gsemac.Net.JavaScript.Obfuscation {

    public sealed class BracketedMemberAccessJsDeobfuscator :
        IJSDeobfuscator {

        // Public members

        public bool IgnoreUnescapedIdentifiers { get; set; } = false;

        public bool TryDeobfuscate(string script, out string result) {

            result = script;

            if (string.IsNullOrWhiteSpace(script))
                return false;

            bool anyReplaced = false;

            // Be careful that we don't confuse array/map accesses for member accesses.
            // We can avoid this for the most part by making sure what we're replacing is a valid identifier (e.g. not a numeric index).
            // It's not a big deal so long as its a valid identifier, though, as a member access will be treated the same as indexing for arrays/maps.

            result = Regex.Replace(script, @"(?<=[\]\)'""a-zA-Z_])(?:\['[^\d][^']*'\]|\[""[^\d][^""]*""\])", m => {

                anyReplaced = true;

                string matchValue = m.Value;

                // Strip the outer quotes from the identifier.

                string identifier = matchValue.Substring(2, matchValue.Length - 4);

                if (IgnoreUnescapedIdentifiers) {

                    // It's common for escaped identifiers to be used with this obfuscation method.
                    // It maybe desirable to ignore unsecaped identifiers so we don't accidentally replace array/map accesses.

                    if (identifier.StartsWith("\\") && DeobfuscatorUtilities.TryUnescapeString(identifier, out string unescapedIdentifier))
                        return $".{identifier}";

                    return matchValue;

                }
                else {

                    return $".{identifier}";

                }

            });

            return anyReplaced;

        }

    }

}
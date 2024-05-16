namespace Gsemac.Net.JavaScript.Obfuscation {

    public class JSDeobfuscatorFactory :
        IJSDeobfuscatorFactory {

        // Public members

        public static JSDeobfuscatorFactory Default => new JSDeobfuscatorFactory();

        public IJSDeobfuscator Create() {

            return new CompositeJsDeobfuscator(new IJSDeobfuscator[] {
                new EscapeSequenceJsDeobfucator(),
                new BracketedMemberAccessJsDeobfuscator(),
                new DeanEdwardsPackerJsDeobfuscator(),
                new SoJsonV4JsDeobfuscator(),
            });

        }

    }

}
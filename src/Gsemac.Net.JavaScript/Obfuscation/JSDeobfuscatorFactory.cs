namespace Gsemac.Net.JavaScript.Obfuscation {

    public class JSDeobfuscatorFactory :
        IJSDeobfuscatorFactory {

        // Public members

        public static JSDeobfuscatorFactory Default => new JSDeobfuscatorFactory();

        public IJSDeobfuscator Create() {

            return new DeanEdwardsPackerDeobfuscator();

        }

    }

}
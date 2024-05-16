namespace Gsemac.Net.JavaScript.Obfuscation {

    public interface IJSDeobfuscator {

        bool TryDeobfuscate(string script, out string result);

    }

}
namespace Gsemac.Net.JavaScript {

    public static class JSString {

        // Public members

        public static string FromCharCode(ushort charCode) {

            return char.ConvertFromUtf32(charCode);

        }
        public static string FromCharCode(int charCode) {

            return FromCharCode((ushort)charCode);

        }

    }

}
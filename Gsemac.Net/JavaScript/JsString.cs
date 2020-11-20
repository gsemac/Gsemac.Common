namespace Gsemac.Net.JavaScript {

    public static class JsString {

        public static string FromCharCode(ushort charCode) {

            return char.ConvertFromUtf32(charCode);

        }

    }

}
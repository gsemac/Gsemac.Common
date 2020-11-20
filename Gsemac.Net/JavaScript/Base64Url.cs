namespace Gsemac.Net.JavaScript {

    public static class Base64Url {

        public static string Decode(string encodedData) {

            string base64 = encodedData.TrimEnd('=');
            base64 = base64.Replace('-', '+');
            base64 = base64.Replace('_', '/');
            base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');

            return base64;

        }

    }

}
using System;
using System.Text;

namespace Gsemac.Net.JavaScript {

    public static class JsWindow {

        public static string Atob(string encodedData) {

            byte[] bytes = Convert.FromBase64String(encodedData);

            return Encoding.GetEncoding(28591).GetString(bytes);


        }
        public static string Btoa(string stringToEncode) {

            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(stringToEncode);

            return Convert.ToBase64String(bytes);

        }

    }

}
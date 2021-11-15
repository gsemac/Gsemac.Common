using Gsemac.Text.Codecs.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Codecs {

    public static class Base64 {

        // Public members

        public static byte[] Encode(byte[] bytesToEncode) {

            return GetEncoder().Encode(bytesToEncode);

        }
        public static byte[] Encode(string stringToEncode, Encoding encoding = null) {

            return GetEncoder().Encode(stringToEncode, encoding);

        }
        public static byte[] Decode(byte[] encodedBytes) {

            return GetDecoder().Decode(encodedBytes);

        }
        public static byte[] Decode(string encodedString, Encoding encoding = null) {

            return GetDecoder().Decode(encodedString, encoding);

        }

        public static string EncodeString(string stringToEncode, Encoding encoding = null) {

            return GetEncoder().EncodeString(stringToEncode, encoding);

        }
        public static string EncodeString(byte[] bytesToEncode, Encoding encoding = null) {

            return GetEncoder().EncodeString(bytesToEncode, encoding);

        }
        public static string DecodeString(string encodedString, Encoding encoding = null) {

            return GetDecoder().DecodeString(encodedString, encoding);

        }
        public static string DecodeString(byte[] encodedBytes, Encoding encoding = null) {

            return GetDecoder().DecodeString(encodedBytes, encoding);

        }

        public static bool IsBase64String(string input) {

            // Slightly modified from the answer given here:
            // https://stackoverflow.com/a/54143400 (Tomas Kubes)

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = PadBase64String(input.Trim());

            return input.Length % 4 == 0 &&
                Regex.IsMatch(input, @"^[a-z0-9\+/]*={0,3}$", RegexOptions.IgnoreCase);

        }

        public static IBinaryEncoder GetEncoder() {

            return new Base64Codec();

        }
        public static IBinaryDecoder GetDecoder() {

            return new Base64Codec();

        }

        // Private members

        private static string PadBase64String(string input) {

            if (string.IsNullOrWhiteSpace(input))
                return input;

            if (input.Length % 4 > 0)
                input = input.PadRight(input.Length + 4 - input.Length % 4, '=');

            return input;

        }

    }

}
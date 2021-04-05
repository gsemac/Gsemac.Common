using Gsemac.Text.Codecs.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Text.Codecs {

    public sealed class Base64 :
        IBinaryCodec {

        // Public members

        public byte[] Encode(byte[] bytesToEncode, int startIndex, int length) {

            return Encoding.UTF8.GetBytes(Convert.ToBase64String(bytesToEncode, startIndex, length));

        }
        public byte[] Decode(byte[] encodedBytes, int startIndex, int length) {

            string base64String = PadBase64String(Encoding.UTF8.GetString(encodedBytes, startIndex, length));

            return Convert.FromBase64String(base64String);

        }

        public static IBinaryEncoder GetEncoder() {

            return new Base64();

        }
        public static IBinaryDecoder GetDecoder() {

            return new Base64();

        }

        public static string EncodeString(string stringToEncode, Encoding encoding = null) {

            return GetEncoder().EncodeString(stringToEncode, encoding);

        }
        public static string DecodeString(string encodedString, Encoding encoding = null) {

            return GetDecoder().DecodeString(encodedString, encoding);

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
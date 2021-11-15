using System;
using System.Text;

namespace Gsemac.Text.Codecs {

    public sealed class Base64Codec :
        IBinaryCodec {

        // Public members

        public byte[] Encode(byte[] bytesToEncode, int startIndex, int length) {

            return Encoding.UTF8.GetBytes(Convert.ToBase64String(bytesToEncode, startIndex, length));

        }
        public byte[] Decode(byte[] encodedBytes, int startIndex, int length) {

            string base64String = PadBase64String(Encoding.UTF8.GetString(encodedBytes, startIndex, length));

            return Convert.FromBase64String(base64String);

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
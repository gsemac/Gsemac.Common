using Gsemac.Text.Codecs.Extensions;
using System;
using System.Globalization;
using System.Text;

namespace Gsemac.Text.Codecs {

    public sealed class Hex :
         IBinaryCodec {

        public byte[] Decode(byte[] encodedBytes, int startIndex, int length) {

            string encodedString = Encoding.UTF8.GetString(encodedBytes, startIndex, length);

            // The hex string must have an even number of digits (since we process a pair of digits at a time).
            // If there are an odd number of digits, pad the string. This should not affect the output.

            if (encodedString.Length % 2 != 0)
                encodedString = '0' + encodedString;

            byte[] decodedBytes = new byte[encodedString.Length / 2];

            for (int index = 0; index < decodedBytes.Length; index++) {

                string byteString = encodedString.Substring(index * 2, 2);

                decodedBytes[index] = byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            }

            return decodedBytes;

        }
        public byte[] Encode(byte[] bytesToEncode, int startIndex, int length) {

            string encodedString = BitConverter.ToString(bytesToEncode, startIndex, length)
                .Replace("-", string.Empty)
                .ToLowerInvariant();

            return Encoding.UTF8.GetBytes(encodedString);

        }

        public static IBinaryEncoder GetEncoder() {

            return new Hex();

        }
        public static IBinaryDecoder GetDecoder() {

            return new Hex();

        }

        public static string EncodeString(string stringToEncode) {

            return GetEncoder().EncodeString(stringToEncode);

        }
        public static string DecodeString(string encodedString) {

            return GetDecoder().DecodeString(encodedString);

        }

    }

}
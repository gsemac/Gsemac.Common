using Gsemac.Text.Codecs.Extensions;
using System.Text;

namespace Gsemac.Text.Codecs {

    public static class Base64Url {

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

        public static IBinaryEncoder GetEncoder() {

            return new Base64UrlCodec();

        }
        public static IBinaryDecoder GetDecoder() {

            return new Base64UrlCodec();

        }

    }

}
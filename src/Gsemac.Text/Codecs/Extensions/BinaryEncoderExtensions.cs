using System.Text;

namespace Gsemac.Text.Codecs.Extensions {

    public static class BinaryEncoderExtensions {

        // Public members

        public static byte[] Encode(this IBinaryEncoder encoder, byte[] bytesToEncode) {

            return encoder.Encode(bytesToEncode, 0, bytesToEncode.Length);

        }
        public static byte[] Encode(this IBinaryEncoder encoder, byte[] bytesToEncode, int startIndex) {

            return encoder.Encode(bytesToEncode, startIndex, bytesToEncode.Length - startIndex);

        }
        public static byte[] Encode(this IBinaryEncoder encoder, string stringToEncode, Encoding encoding = null) {

            if (encoding is null)
                encoding = defaultEncoding;

            byte[] byteToEncode = encoding.GetBytes(stringToEncode);

            return encoder.Encode(byteToEncode);

        }
        public static string EncodeString(this IBinaryEncoder encoder, string stringToEncode, Encoding encoding = null) {

            if (encoding is null)
                encoding = defaultEncoding;

            return encoding.GetString(encoder.Encode(stringToEncode, encoding));

        }

        // Private members

        private static readonly Encoding defaultEncoding = Encoding.UTF8;

    }

}
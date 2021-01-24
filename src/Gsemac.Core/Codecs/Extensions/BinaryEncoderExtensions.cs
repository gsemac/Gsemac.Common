using System.Text;

namespace Gsemac.Core.Codecs.Extensions {

    public static class BinaryEncoderExtensions {

        public static byte[] Encode(this IBinaryEncoder encoder, byte[] bytesToEncode) {

            return encoder.Encode(bytesToEncode, 0, bytesToEncode.Length);

        }
        public static byte[] Encode(this IBinaryEncoder encoder, byte[] bytesToEncode, int startIndex) {

            return encoder.Encode(bytesToEncode, startIndex, bytesToEncode.Length - startIndex);

        }
        public static string EncodeString(this IBinaryEncoder encoder, string stringToEncode, Encoding encoding = null) {

            if (encoding is null)
                encoding = Encoding.UTF8;

            byte[] byteToEncode = encoding.GetBytes(stringToEncode);

            return encoding.GetString(encoder.Encode(byteToEncode));

        }

    }

}
using System.Text;

namespace Gsemac.Core.Extensions {

    public static class BinaryEncoderExtensions {

        public static byte[] Encode(this IBinaryEncoder encoder, byte[] bytesToEncode) {

            return encoder.Encode(bytesToEncode, 0, bytesToEncode.Length);

        }
        public static string EncodeString(this IBinaryEncoder encoder, string stringToEncode, Encoding encoding = null) {

            if (encoding is null)
                encoding = Encoding.UTF8;

            byte[] byteToEncode = encoding.GetBytes(stringToEncode);

            return encoding.GetString(encoder.Encode(byteToEncode));

        }

    }

}
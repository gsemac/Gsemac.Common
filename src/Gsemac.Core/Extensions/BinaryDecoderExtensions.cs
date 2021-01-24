using System.Text;

namespace Gsemac.Core.Extensions {

    public static class BinaryDecoderExtensions {

        public static byte[] Decode(this IBinaryDecoder decoder, byte[] encodedBytes) {

            return decoder.Decode(encodedBytes, 0, encodedBytes.Length);

        }
        public static string DecodeString(this IBinaryDecoder decoder, string encodedString, Encoding encoding = null) {

            if (encoding is null)
                encoding = Encoding.UTF8;

            byte[] encodedBytes = encoding.GetBytes(encodedString);

            return encoding.GetString(decoder.Decode(encodedBytes));

        }

    }

}
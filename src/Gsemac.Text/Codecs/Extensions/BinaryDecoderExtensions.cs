using System.Text;

namespace Gsemac.Text.Codecs.Extensions {

    public static class BinaryDecoderExtensions {

        public static byte[] Decode(this IBinaryDecoder decoder, byte[] encodedBytes) {

            return decoder.Decode(encodedBytes, 0, encodedBytes.Length);

        }
        public static byte[] Decode(this IBinaryDecoder decoder, byte[] encodedBytes, int startIndex) {

            return decoder.Decode(encodedBytes, startIndex, encodedBytes.Length - startIndex);

        }
        public static byte[] Decode(this IBinaryDecoder decoder, string encodedString, Encoding encoding = null) {

            if (encoding is null)
                encoding = Encoding.UTF8;

            byte[] encodedBytes = encoding.GetBytes(encodedString);

            return decoder.Decode(encodedBytes);

        }
        public static string DecodeString(this IBinaryDecoder decoder, string encodedString, Encoding encoding = null) {

            return encoding.GetString(decoder.Decode(encodedString, encoding));

        }

    }

}
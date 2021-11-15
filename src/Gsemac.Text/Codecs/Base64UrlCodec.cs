using System;
using System.Text;

namespace Gsemac.Text.Codecs {

    public sealed class Base64UrlCodec :
        IBinaryCodec {

        // Public members

        public byte[] Decode(byte[] encodedBytes, int startIndex, int length) {

            if (encodedBytes is null)
                throw new ArgumentNullException(nameof(encodedBytes));

            string base64UrlString = Encoding.UTF8.GetString(encodedBytes, startIndex, length);

            StringBuilder sb = new StringBuilder(base64UrlString);

            sb.Replace('-', '+');
            sb.Replace('_', '/');

            return Base64.Decode(Encoding.UTF8.GetBytes(sb.ToString()));

        }
        public byte[] Encode(byte[] bytesToEncode, int startIndex, int length) {

            if (bytesToEncode is null)
                throw new ArgumentNullException(nameof(bytesToEncode));

            string base64String = Encoding.UTF8.GetString(Base64.GetEncoder().Encode(bytesToEncode, startIndex, length));

            StringBuilder sb = new StringBuilder(base64String);

            sb.Replace('+', '-');
            sb.Replace('/', '_');

            return Encoding.UTF8.GetBytes(sb.ToString());

        }

    }

}

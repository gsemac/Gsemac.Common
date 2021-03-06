﻿using Gsemac.Text.Codecs.Extensions;
using System.Text;

namespace Gsemac.Text.Codecs {

    public sealed class Base64Url :
        IBinaryCodec {

        public byte[] Decode(byte[] encodedBytes, int startIndex, int length) {

            string base64UrlString = Encoding.UTF8.GetString(encodedBytes, startIndex, length);

            StringBuilder sb = new StringBuilder(base64UrlString);

            sb.Replace('-', '+');
            sb.Replace('_', '/');

            return Base64.GetDecoder().Decode(Encoding.UTF8.GetBytes(sb.ToString()));

        }
        public byte[] Encode(byte[] bytesToEncode, int startIndex, int length) {

            string base64String = Encoding.UTF8.GetString(Base64.GetEncoder().Encode(bytesToEncode, startIndex, length));

            StringBuilder sb = new StringBuilder(base64String);

            sb.Replace('+', '-');
            sb.Replace('/', '_');

            return Encoding.UTF8.GetBytes(sb.ToString());

        }

        public static IBinaryEncoder GetEncoder() {

            return new Base64Url();

        }
        public static IBinaryDecoder GetDecoder() {

            return new Base64Url();

        }

        public static string EncodeString(string stringToEncode, Encoding encoding = null) {

            return GetEncoder().EncodeString(stringToEncode, encoding);

        }
        public static string DecodeString(string encodedString, Encoding encoding = null) {

            return GetDecoder().DecodeString(encodedString, encoding);

        }

    }

}
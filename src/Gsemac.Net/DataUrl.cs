using Gsemac.IO;
using Gsemac.Text.Codecs;
using Gsemac.Text.Codecs.Extensions;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net {

    public class DataUrl :
        IDataUrl {

        // Public members

        public IMimeType MimeType { get; }
        public bool IsBase64Encoded { get; } = false;
        public int DataLength => data.Length;

        public DataUrl(IMimeType mimeType, byte[] data) :
            this(mimeType, data, false) {
        }
        public DataUrl(IMimeType mimeType, byte[] data, bool base64Encode) {

            if (mimeType is null)
                throw new ArgumentNullException(nameof(mimeType));

            if (data is null)
                throw new ArgumentNullException(nameof(data));

            this.data = data;

            MimeType = mimeType;
            IsBase64Encoded = base64Encode;

        }

        public Stream GetDataStream() {

            return new MemoryStream(data, writable: false);

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append("data:");

            if (MimeType is object)
                sb.Append(MimeType.ToString());

            if (IsBase64Encoded)
                sb.Append(";base64");

            sb.Append(',');

            if (IsBase64Encoded)
                sb.Append(Base64.EncodeString(data));
            else
                sb.Append(Uri.EscapeDataString(Encoding.UTF8.GetString(data)));

            return sb.ToString();

        }

        public static DataUrl Parse(string value) {

            if (TryParse(value, out DataUrl result))
                return result;

            throw new ArgumentException(Properties.ExceptionMessages.MalformedDataUrl);

        }
        public static bool TryParse(string value, out DataUrl dataUrl) {

            dataUrl = null;

            if (!string.IsNullOrWhiteSpace(value)) {

                Match match = new Regex("^data:(?<mimetype>.*?)(?<base64>;base64)?,(?<data>.*?$)")
                    .Match(value);

                if (match.Success) {

                    string mimeTypeStr = match.Groups["mimetype"].Value;
                    string dataStr = match.Groups["data"].Value;
                    bool base64Encoded = !string.IsNullOrWhiteSpace(match.Groups["base64"].Value);

                    // If the MIME type is omitted, it defaults to "text/plain;charset=US-ASCII".
                    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs

                    if (string.IsNullOrWhiteSpace(mimeTypeStr))
                        mimeTypeStr = "text/plain;charset=US-ASCII";

                    if (IO.MimeType.TryParse(mimeTypeStr, out MimeType mimeType)) {

                        byte[] data = base64Encoded ?
                            Base64.GetDecoder().Decode(dataStr) :
                            Encoding.UTF8.GetBytes(Uri.UnescapeDataString(dataStr));

                        dataUrl = new DataUrl(mimeType, data, base64Encode: base64Encoded);

                    }

                }

            }

            return dataUrl is object;

        }

        // Private members

        private readonly byte[] data;

    }

}
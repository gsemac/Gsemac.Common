using System;
using System.Net;

namespace Gsemac.Net {

    internal sealed class DataWebRequest :
        WebRequest {

        // Public members

        public override long ContentLength {
            get => dataUrl.DataLength;
        }
        public override string ContentType {
            get => dataUrl.MimeType.ToString();
        }
        public override WebHeaderCollection Headers { get; set; } = new WebHeaderCollection();
        public override IWebProxy Proxy { get; set; } = WebRequestUtilities.GetDefaultWebProxy();

        public DataWebRequest(Uri uri) {

            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            dataUrl = DataUrl.Parse(uri.AbsoluteUri);

        }

        public override WebResponse GetResponse() {

            return new DataWebResponse(dataUrl);

        }

        // Private members

        private readonly IDataUrl dataUrl;

    }

}
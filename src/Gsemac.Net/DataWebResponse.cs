using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace Gsemac.Net {

    internal sealed class DataWebResponse :
        WebResponse {

        // Public members

        public override long ContentLength {
            get => dataUrl.DataLength;
        }
        public override string ContentType {
            get => dataUrl.MimeType.ToString();
        }
        public override WebHeaderCollection Headers { get; } = new WebHeaderCollection();

        public DataWebResponse(IDataUrl dataUrl) {

            if (dataUrl is null)
                throw new ArgumentNullException(nameof(dataUrl));

            this.dataUrl = dataUrl;

            Headers.Add(HttpResponseHeader.ContentLength, dataUrl.DataLength.ToString(CultureInfo.InvariantCulture));
            Headers.Add(HttpResponseHeader.ContentType, dataUrl.MimeType.ToString());

        }

        public override Stream GetResponseStream() {

            return dataUrl.GetDataStream();

        }

        // Private members

        private readonly IDataUrl dataUrl;

    }

}
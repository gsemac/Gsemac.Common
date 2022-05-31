using System;
using System.IO;

namespace Gsemac.Net.Http {

    public class HttpResponseWriter :
        HttpWriterBase,
        IHttpResponseWriter {

        // Public members

        public HttpResponseWriter(Stream stream) :
            base(stream) {
        }

        public void WriteStatusLine(IHttpStatusLine statusLine) {

            if (statusLine is null)
                throw new ArgumentNullException(nameof(statusLine));

            WriteStartLine(statusLine);

        }

    }

}
using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Net {

    public class HttpResponseReader :
        IHttpResponseReader {

        // Public members

        public HttpResponseReader(Stream responseStream) {

            reader = new UnbufferedStreamReader(responseStream);

        }

        public IHttpStatusLine ReadStatusLine() {

            string statusLine = reader.ReadLine();

            return HttpStatusLine.Parse(statusLine);

        }
        public IEnumerable<IHttpHeader> ReadHeaders() {

            List<IHttpHeader> headers = new List<IHttpHeader>();

            string header;

            while (!string.IsNullOrWhiteSpace(header = reader.ReadLine()))
                headers.Add(HttpHeader.Parse(header));

            return headers;

        }
        public string ReadBody() {

            return reader.ReadToEnd();

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    reader.Dispose();

                }

                disposedValue = true;
            }

        }

        // Private members

        private readonly UnbufferedStreamReader reader;
        private bool disposedValue;

    }

}
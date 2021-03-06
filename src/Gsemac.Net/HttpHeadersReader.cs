﻿using Gsemac.IO;
using System.IO;
using System.Text;

namespace Gsemac.Net {

    public sealed class HttpHeadersReader :
        IHttpHeadersReader {

        // Public members

        public HttpHeadersReader(Stream responseStream) {

            // HTTP headers must be encoded using ISO-8859-1.

            reader = new UnbufferedStreamReader(responseStream, Encoding.GetEncoding("ISO-8859-1"));

        }

        public bool ReadStatusLine(out IHttpStatusLine statusLine) {

            statusLine = GetStatusLine();

            return statusLine is object;

        }
        public bool ReadHeader(out IHttpHeader header) {

            string nextLine;

            if (!string.IsNullOrEmpty(bufferedLine)) {

                nextLine = bufferedLine;
                bufferedLine = string.Empty;

            }
            else {

                nextLine = reader.ReadLine();

            }

            if (!string.IsNullOrWhiteSpace(nextLine)) {

                header = HttpHeader.Parse(nextLine);

                return true;

            }
            else {

                header = null;

                return false;

            }

        }

        public void Dispose() {

            if (!disposedValue) {

                reader.Dispose();

                disposedValue = true;
            }

        }

        // Private members

        private readonly UnbufferedStreamReader reader;
        private string bufferedLine;
        private bool disposedValue;

        private IHttpStatusLine GetStatusLine() {

            IHttpStatusLine statusLine = null;

            if (string.IsNullOrEmpty(bufferedLine)) {

                bufferedLine = reader.ReadLine();

                if (HttpStatusLine.TryParse(bufferedLine, out statusLine))
                    bufferedLine = string.Empty;

            }

            return statusLine;

        }

    }

}
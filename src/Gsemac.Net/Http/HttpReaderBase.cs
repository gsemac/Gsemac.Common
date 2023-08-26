using Gsemac.IO;
using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gsemac.Net.Http
{

    public abstract class HttpReaderBase :
        IHttpReader {

        // Public members

        public IHttpStartLine StartLine => startLine.Value;
        public IEnumerable<IHttpHeader> Headers => headers.Value;

        public bool TryReadStartLine(out IHttpStartLine startLine) {

            // When the user chooses to manually read a start line, we'll reset the reader.
            // This is for the case where we're reading from a stream that contains a list of concatenated requests (e.g. from cURL).

            Reset();

            startLine = StartLine;

            return startLine is object;

        }
        public bool TryReadHeader(out IHttpHeader header) {

            ResetHeaders();

            header = Headers.FirstOrDefault();

            return header is object;

        }

        public Stream GetBodyStream() {

            EnsureHttpHeadIsRead();

            return reader.BaseStream;

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected HttpReaderBase(Stream httpStream) {

            if (httpStream is null)
                throw new ArgumentNullException(nameof(httpStream));

            // HTTP headers must be encoded using ISO-8859-1.

            reader = new UnbufferedStreamReader(httpStream, Encoding.GetEncoding("ISO-8859-1"));

            Reset();

        }

        protected abstract IHttpStartLine ParseStartLine(string startLine);

        protected void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    reader.Dispose();

                }

                disposedValue = true;

            }

        }

        // Private members

        private readonly UnbufferedStreamReader reader;
        private Lazy<IHttpStartLine> startLine;
        private Lazy<IEnumerable<IHttpHeader>> headers;
        private string bufferedLine;
        private bool disposedValue;

        private IHttpStartLine ReadStartLineInternal() {

            if (string.IsNullOrEmpty(bufferedLine)) {

                bufferedLine = reader.ReadLine();

                IHttpStartLine startLine = ParseStartLine(bufferedLine);

                if (startLine is object)
                    bufferedLine = string.Empty;

                return startLine;

            }

            return null;

        }
        private IHttpHeader ReadHeaderInternal() {

            string nextLine;

            if (!string.IsNullOrEmpty(bufferedLine)) {

                nextLine = bufferedLine;
                bufferedLine = string.Empty;

            }
            else {

                nextLine = reader.ReadLine();

            }

            if (!string.IsNullOrWhiteSpace(nextLine)) {

                return HttpHeader.Parse(nextLine);

            }
            else {

                return null;

            }

        }
        private IEnumerable<IHttpHeader> ReadHeadersInternal() {

            EnsureStartLineIsRead();

            while (!reader.EndOfStream) {

                IHttpHeader header = ReadHeaderInternal();

                if (header is object)
                    yield return header;
                else
                    break;

            }

        }

        private void EnsureStartLineIsRead() {

            if (!startLine.IsValueCreated)
                _ = startLine.Value;

        }
        private void EnsureHttpHeadIsRead() {

            EnsureStartLineIsRead();

            if (!headers.IsValueCreated)
                _ = headers.Value.ToArray();

        }

        private void Reset() {

            startLine = new Lazy<IHttpStartLine>(ReadStartLineInternal);

            ResetHeaders();

        }
        private void ResetHeaders() {

            headers = new Lazy<IEnumerable<IHttpHeader>>(ReadHeadersInternal);

        }

    }

}
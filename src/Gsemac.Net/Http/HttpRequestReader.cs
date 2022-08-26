using System.IO;

namespace Gsemac.Net.Http {

    public sealed class HttpRequestReader :
        HttpReaderBase,
        IHttpRequestReader {

        // Public members

        IHttpRequestLine IHttpRequestReader.StartLine => (IHttpRequestLine)StartLine;

        public HttpRequestReader(Stream httpStream) :
            base(httpStream) {
        }

        public bool TryReadStartLine(out IHttpRequestLine startLine) {

            if (TryReadStartLine(out IHttpStartLine result)) {

                startLine = (IHttpRequestLine)result;

                return true;

            }
            else {

                startLine = null;

                return false;

            }

        }

        // Protected members

        protected override IHttpStartLine ParseStartLine(string startLine) {

            if (HttpRequestLine.TryParse(startLine, out HttpRequestLine result))
                return result;

            return null;

        }

    }

}
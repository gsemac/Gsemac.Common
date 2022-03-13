using System.IO;

namespace Gsemac.Net {

    public sealed class HttpRequestReader :
        HttpReaderBase,
        IHttpRequestReader {

        // Public members

        IHttpRequestLine IHttpRequestReader.StartLine => (IHttpRequestLine)StartLine;

        public HttpRequestReader(Stream httpStream) :
            base(httpStream) {
        }

        public bool ReadStartLine(out IHttpRequestLine startLine) {

            if (ReadStartLine(out IHttpStartLine result)) {

                startLine = (IHttpRequestLine)result;

                return true;

            }
            else {

                startLine = null;

                return true;

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
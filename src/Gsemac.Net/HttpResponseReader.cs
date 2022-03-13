using System.IO;

namespace Gsemac.Net {

    public sealed class HttpResponseReader :
        HttpReaderBase,
        IHttpResponseReader {

        // Public members

        IHttpStatusLine IHttpResponseReader.StartLine => (IHttpStatusLine)StartLine;

        public HttpResponseReader(Stream httpStream) :
            base(httpStream) {
        }

        public bool ReadStartLine(out IHttpStatusLine startLine) {

            if (ReadStartLine(out IHttpStartLine result)) {

                startLine = (IHttpStatusLine)result;

                return true;

            }
            else {

                startLine = null;

                return true;

            }

        }

        // Protected members

        protected override IHttpStartLine ParseStartLine(string startLine) {

            if (HttpStatusLine.TryParse(startLine, out HttpStatusLine result))
                return result;

            return null;

        }

    }

}
﻿using System.IO;

namespace Gsemac.Net.Http {

    public sealed class HttpResponseReader :
        HttpReaderBase,
        IHttpResponseReader {

        // Public members

        IHttpStatusLine IHttpResponseReader.StartLine => (IHttpStatusLine)StartLine;

        public HttpResponseReader(Stream httpStream) :
            base(httpStream) {
        }

        public bool TryReadStartLine(out IHttpStatusLine startLine) {

            if (TryReadStartLine(out IHttpStartLine result)) {

                startLine = (IHttpStatusLine)result;

                return true;

            }
            else {

                startLine = null;

                return false;

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
using Gsemac.Text;
using System;
using System.IO;
using System.Text;

namespace Gsemac.Net.Http {

    public abstract class HttpWriterBase :
        IHttpWriter {

        // Public members

        public void WriteBody(Stream body) {

            if (body is null)
                throw new ArgumentNullException(nameof(body));

            WriteNewLine();

            body.CopyTo(stream);

        }
        public void WriteHeader(IHttpHeader header) {

            if (header is null)
                throw new ArgumentNullException(nameof(header));

            WriteString(header.ToString());
            WriteNewLine();

        }

        // Protected members

        protected HttpWriterBase(Stream stream) {

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            this.stream = stream;

        }

        protected void WriteStartLine(IHttpStartLine startLine) {

            WriteString(startLine.ToString());
            WriteNewLine();

        }

        // Private members

        private readonly Stream stream;
        private readonly Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

        private void WriteString(string value) {

            using (Stream inputStream = StringUtilities.StringToStream(value, encoding))
                inputStream.CopyTo(stream);

        }
        private void WriteNewLine() {

            WriteString("\r\n");

        }

    }

}
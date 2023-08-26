using Gsemac.Net.Http.Headers;
using System;
using System.IO;
using System.Text;

namespace Gsemac.Net.Http {

    public static class HttpWriterExtensions {

        // Public members

        public static void WriteHeader(this IHttpWriter httpWriter, string header, string value) {

            if (httpWriter is null)
                throw new ArgumentNullException(nameof(httpWriter));

            httpWriter.WriteHeader(new HttpHeader(header, value));

        }
        public static void WriteBody(this IHttpWriter httpWriter, string body) {

            if (httpWriter is null)
                throw new ArgumentNullException(nameof(httpWriter));

            if (body is null)
                throw new ArgumentNullException(nameof(body));

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
                httpWriter.WriteBody(stream);

        }

    }

}
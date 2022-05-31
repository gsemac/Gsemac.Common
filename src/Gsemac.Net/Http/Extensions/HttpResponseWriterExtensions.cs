using System;
using System.Net;

namespace Gsemac.Net.Http.Extensions {

    public static class HttpResponseWriterExtensions {

        // Public members

        public static void WriteStatusLine(this IHttpResponseWriter httpWriter, HttpStatusCode statusCode) {

            if (httpWriter is null)
                throw new ArgumentNullException(nameof(httpWriter));

            httpWriter.WriteStatusLine(new HttpStatusLine(statusCode));

        }

        public static void WriteHeader(this IHttpResponseWriter httpWriter, HttpResponseHeader header, string value) {

            httpWriter.WriteHeader(new HttpHeader(header, value));

        }

    }

}
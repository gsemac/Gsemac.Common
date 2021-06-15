using Gsemac.Collections.Extensions;
using Gsemac.Net.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Gsemac.Net {

    // Despite the RFC stating that header ordering doesn't matter, some CDNs will block requests if the headers are not in a certain order (e.g. Cloudflare).
    // See https://sansec.io/research/http-header-order-is-important
    // This workaround is based on the answer given here: https://stackoverflow.com/a/63762847/5383169 (Jason)

    public class SortHeadersHandler :
        DelegatingWebRequestHandler {

        // Public members

        public SortHeadersHandler() :
            base() {
        }
        public SortHeadersHandler(WebRequestHandler innerHandler) :
            base(innerHandler) {
        }

        // Protected members

        protected internal override WebResponse Send(WebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            SetWebHeaderCollection(request);

            return base.Send(request, cancellationToken);

        }

        // Private members

        private class ReorderedWebHeaderCollection :
            WebHeaderCollection {

            // Public members

            public ReorderedWebHeaderCollection(HttpWebRequest webRequest) {

                // Store the web request so we can use its properties to generate headers even if they're changed after this object was instantiated.

                this.webRequest = webRequest;

                // The original headers will be lost when we set the new WebHeaderCollection, so we need to copy them immediately.

                webRequest.Headers.CopyTo(this);

            }

            public override string ToString() {

                IList<IHttpHeader> headers = new List<IHttpHeader>(this.GetHeaders());

                // Add required headers that are normally added automatically (they won't be added by HttpWebRequest).

                AddRequiredHeaders(headers, new HttpWebRequestAdapter(webRequest));

                StringBuilder sb = new StringBuilder();

                foreach (IHttpHeader header in headers.OrderBy(h => GetHeaderOrder(h))) {

                    sb.Append(header.Name);
                    sb.Append(": ");
                    sb.Append(header.Value);
                    sb.Append("\r\n");

                }

                sb.Append("\r\n");

                return sb.ToString();

            }

            // Private members

            private readonly HttpWebRequest webRequest;



        }

        private void SetWebHeaderCollection(WebRequest webRequest) {

            webRequest = webRequest.GetInnermostWebRequest();

            if (webRequest is HttpWebRequest httpWebRequest) {

                // Even though the Headers property is assignable, HttpWebRequest simply copies the headers out of it instead of replacing its own collection.
                // As a result, the only way of replacing the WebHeaderCollection is by replacing the field through reflection.

                FieldInfo fieldInfo = typeof(HttpWebRequest).GetField("_HttpRequestHeaders", BindingFlags.Instance | BindingFlags.NonPublic);

                if (fieldInfo is object)
                    fieldInfo.SetValue(httpWebRequest, new ReorderedWebHeaderCollection(httpWebRequest));

            }
            else {

                // For a generic request, just reorder and reinsert each of the headers.

                IList<IHttpHeader> headers = new List<IHttpHeader>(webRequest.Headers.GetHeaders());

                if (webRequest is IHttpWebRequest iHttpWebRequest)
                    AddRequiredHeaders(headers, iHttpWebRequest);

                webRequest.Headers.Clear();

                webRequest.Headers.TrySetHeaders(headers.OrderBy(header => GetHeaderOrder(header)));

            }

        }

        private static int GetHeaderOrder(IHttpHeader header) {

            string[] headerOrder = new[] {
                    "host",
                    "connection",
                    "accept",
                    "user-agent",
                };

            int order = headerOrder.IndexOf(header.Name.ToLowerInvariant());

            return order < 0 ?
                headerOrder.Length :
                order;

        }
        private static void AddRequiredHeaders(IList<IHttpHeader> headers, IHttpWebRequest webRequest) {

            if (!headers.Any(h => h.Name.Equals("Host", StringComparison.OrdinalIgnoreCase)))
                headers.Add(new HttpHeader("Host", Url.GetHostname(webRequest.RequestUri.AbsoluteUri)));

            if (webRequest.KeepAlive && !headers.Any(h => h.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase)))
                headers.Add(new HttpHeader("Connection", "keep-alive"));

        }

    }

}
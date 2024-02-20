using Gsemac.Net.Extensions;
using Gsemac.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Gsemac.Net.Http {

    // Despite the RFC stating that header ordering doesn't matter, some CDNs will block requests if the headers are not in a certain order (e.g. Cloudflare).
    // See https://sansec.io/research/http-header-order-is-important
    // This workaround is based on the answer given here: https://stackoverflow.com/a/63762847/5383169 (Jason)

    public class SortHeadersHandler :
        DelegatingWebRequestHandler {

        // Public members

        public SortHeadersHandler() :
            base() {
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

            public ReorderedWebHeaderCollection(WebRequest webRequest) {

                if (webRequest is null)
                    throw new ArgumentNullException(nameof(webRequest));

                // Store the web request so we can use its properties to generate headers even if they're changed after this object was instantiated.

                this.webRequest = webRequest;

                // The original headers will be lost when we set the new WebHeaderCollection, so we need to copy them immediately.

                webRequest.Headers.CopyTo(this);

            }

            public override string ToString() {

                IList<IHttpHeader> headers = new List<IHttpHeader>(this.GetAll());

                // Add required headers that are normally added automatically (they won't be added by HttpWebRequest).

                AddRequiredHeaders(headers, webRequest.AsHttpWebRequest());

                StringBuilder sb = new StringBuilder();

                string userAgent = headers.Where(header => header.Name.Equals("user-agent", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Value ?? string.Empty;

                foreach (IHttpHeader header in headers.OrderBy(h => h, GetHeaderComparer(userAgent))) {

                    sb.Append(header.Name);
                    sb.Append(": ");
                    sb.Append(header.Value);
                    sb.Append("\r\n");

                }

                sb.Append("\r\n");

                return sb.ToString();

            }

            // Private members

            private readonly WebRequest webRequest;

            private static void AddRequiredHeaders(IList<IHttpHeader> headers, IHttpWebRequest webRequest) {

                if (headers is null)
                    throw new ArgumentNullException(nameof(headers));

                if (webRequest is null)
                    throw new ArgumentNullException(nameof(webRequest));

                // Add the "Host" header.

                if (!headers.Any(header => header.Name.Equals("Host", StringComparison.OrdinalIgnoreCase)))
                    headers.Add(new HttpHeader("Host", webRequest.RequestUri.Authority));

                // Add the "Connection" header.

                // We will replace the connection header if it already exists with a lowercase variant.
                // While .NET likes to send the value in proper case ("Keep-Alive"), mainstream browsers (Chrome, Firefox) will use lowercase ("keep-alive").

                IHttpHeader connectionHeader = headers.Where(header => header.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()
                    ?? new HttpHeader("Connection", webRequest.KeepAlive ? "keep-alive" : "close");

                headers.Remove(connectionHeader);

                headers.Add(new HttpHeader("Connection", connectionHeader.Value.ToLowerInvariant()));

            }
            private static IComparer<IHttpHeader> GetHeaderComparer(string userAgent) {

                // Different web browsers will send their headers in different orders.

                if (!string.IsNullOrWhiteSpace(userAgent)) {

                    // Note that this will also match other Chromium-based browsers (e.g. Edge).

                    if (userAgent.Contains(" Chrome/"))
                        return new ChromiumHttpHeaderComparer();

                    if (userAgent.Contains(" Firefox/"))
                        return new FirefoxHttpHeaderComparer();

                }

                // Default to the Chrome header order when we can't determine the user agent.

                return new ChromiumHttpHeaderComparer();

            }

        }

        private void SetWebHeaderCollection(WebRequest webRequest) {

            if (webRequest is null)
                throw new ArgumentNullException(nameof(webRequest));

            webRequest = HttpWebRequestUtilities.GetInnermostWebRequest(webRequest);

            if (webRequest is HttpWebRequest httpWebRequest) {

                // Even though the Headers property is assignable, HttpWebRequest simply copies the headers out of it instead of replacing its own collection.
                // As a result, the only way of replacing the WebHeaderCollection is by replacing the field through reflection.

                FieldInfo fieldInfo = typeof(HttpWebRequest).GetField("_HttpRequestHeaders", BindingFlags.Instance | BindingFlags.NonPublic);

                if (fieldInfo is object)
                    fieldInfo.SetValue(httpWebRequest, new ReorderedWebHeaderCollection(httpWebRequest));

            }
            else {

                // For a generic request, just assign a new WebHeaderCollection.

                try {

                    webRequest.Headers = new ReorderedWebHeaderCollection(webRequest);

                }
                catch (NotImplementedException) {

                    // Not all WebRequest implementations will implement the Headers property. This is fine, and can be ignored.
                    // https://docs.microsoft.com/en-us/dotnet/api/system.net.webrequest.headers
                    // For example, if we're attempting to download a local file (with FileWebRequest), the Headers property will throw.

                }

            }

        }

    }

}
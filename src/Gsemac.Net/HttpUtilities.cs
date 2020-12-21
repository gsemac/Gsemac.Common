using System;
using System.Net;

namespace Gsemac.Net {

    public static class HttpUtilities {

        public static HttpStatusCode? GetStatusCode(IHttpWebRequest webRequest) {

            if (webRequest is null)
                throw new ArgumentNullException(nameof(webRequest));

            HttpStatusCode? statusCode = null;

            // Attempt to make a HEAD request first. If it fails, we'll fall back to making a GET request.

            webRequest.AllowAutoRedirect = false;

            foreach (string method in new[] { "HEAD", "GET" }) {

                webRequest.Method = method;

                try {

                    using (WebResponse webResponse = webRequest.GetResponse())
                        statusCode = (webResponse as IHttpWebResponse)?.StatusCode;
                    
                }
                catch (WebException ex) {

                    using (WebResponse webResponse = ex.Response)
                        statusCode = (webResponse as IHttpWebResponse)?.StatusCode;

                }

                if (statusCode.HasValue && statusCode != HttpStatusCode.MethodNotAllowed)
                    break;

            }

            return statusCode;

        }

    }

}
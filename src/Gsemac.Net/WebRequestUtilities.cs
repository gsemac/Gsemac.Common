using Gsemac.Net.Extensions;
using System;
using System.Globalization;
using System.Net;

namespace Gsemac.Net {

    public static class WebRequestUtilities {

        public static HttpStatusCode? GetHttpStatusCode(IHttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            HttpStatusCode? statusCode = null;

            bool originalAllowAutoRedirect = httpWebRequest.AllowAutoRedirect;
            string originalMethod = httpWebRequest.Method;

            // Attempt to make a HEAD request first. If it fails, we'll fall back to making a GET request.

            httpWebRequest.AllowAutoRedirect = false;

            foreach (string method in new[] { "HEAD", "GET" }) {

                httpWebRequest.Method = method;

                try {

                    using (WebResponse webResponse = httpWebRequest.GetResponse())
                        statusCode = (webResponse as IHttpWebResponse)?.StatusCode;

                }
                catch (WebException ex) {

                    using (WebResponse webResponse = ex.Response)
                        statusCode = (webResponse as IHttpWebResponse)?.StatusCode;

                }

                if (statusCode.HasValue && statusCode != HttpStatusCode.MethodNotAllowed)
                    break;

            }

            // Restore original configuration.

            httpWebRequest.AllowAutoRedirect = originalAllowAutoRedirect;
            httpWebRequest.Method = originalMethod;

            return statusCode;

        }
        public static HttpStatusCode? GetHttpStatusCode(HttpWebRequest httpWebRequest) {

            return GetHttpStatusCode(new HttpWebRequestWrapper(httpWebRequest));

        }

        public static WebResponse FollowHttpRedirects(IHttpWebRequest httpWebRequest, IHttpWebRequestFactory httpWebRequestFactory) {

            // While redirections can be handled automatically be enabling "AllowAutoRedirect", the default implementation ignores the set-cookie header of intermediate responses.
            // This implementation preserves cookies set throughout the entire chain of requests.

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            int maximumRedirections = httpWebRequest.MaximumAutomaticRedirections;
            bool originalAllowAutoRedirect = httpWebRequest.AllowAutoRedirect;

            httpWebRequest.AllowAutoRedirect = false;

            try {

                IHttpWebResponse response = null;

                for (int redirections = 0; redirections < maximumRedirections; ++redirections) {

                    response = httpWebRequest.GetResponse() as IHttpWebResponse;

                    if (response is object && (response.StatusCode == HttpStatusCode.Ambiguous ||
                        response.StatusCode == HttpStatusCode.Moved ||
                        response.StatusCode == HttpStatusCode.Redirect ||
                        response.StatusCode == HttpStatusCode.RedirectMethod ||
                        response.StatusCode == HttpStatusCode.RedirectKeepVerb)) {

                        if (response.Headers.TryGetHeaderValue(HttpResponseHeader.Location, out string locationValue) && !string.IsNullOrWhiteSpace(locationValue)) {

                            // Follow the redirect to the new location.

                            if (Uri.TryCreate(httpWebRequest.RequestUri, locationValue, out Uri locationUri) && (locationUri.Scheme == Uri.UriSchemeHttp || locationUri.Scheme == Uri.UriSchemeHttps)) {

                                Uri originalUri = httpWebRequest.RequestUri;
                                string originalMethod = httpWebRequest.Method;

                                httpWebRequest = httpWebRequestFactory.Create(locationUri)
                                    .WithOptions(HttpWebRequestOptions.FromHttpWebRequest(httpWebRequest));

                                if (response.StatusCode == HttpStatusCode.RedirectKeepVerb)
                                    httpWebRequest.Method = originalMethod;

                                response.Close();

                                // If the host is the same, use the full path as the referer.
                                // However, if the hosts are different, we'll just use the root path as the referer (this is how Chrome handles it).

                                if (Uri.Compare(originalUri, locationUri, UriComponents.Host, UriFormat.Unescaped, StringComparison.OrdinalIgnoreCase) == 0)
                                    httpWebRequest.Referer = originalUri.AbsoluteUri;
                                else
                                    httpWebRequest.Referer = originalUri.GetLeftPart(UriPartial.Authority) + "/";

                            }
                            else {

                                // The location URI was malformed.

                                response.Close();

                                throw new WebException("Cannot handle redirect from HTTP/HTTPS protocols to other dissimilar ones.", null, WebExceptionStatus.ProtocolError, response as WebResponse);

                            }

                        }
                        else {

                            // We did not receive a location header even though we were supposed to.

                            response.Close();

                            throw new WebException($"The remote server returned an error: {response.StatusCode}.", null, WebExceptionStatus.ProtocolError, response as WebResponse);

                        }

                    }
                    else {

                        // We did not receive an HTTP redirection.

                        return response as WebResponse;

                    }

                }

                // If we get here, we reached the maximum number of redirections.

                response?.Close();

                throw new WebException("Too many automatic redirections were attempted.", null, WebExceptionStatus.ProtocolError, response as WebResponse);

            }
            finally {

                httpWebRequest.AllowAutoRedirect = originalAllowAutoRedirect;

            }

        }
        public static WebResponse FollowHttpRedirects(HttpWebRequest httpWebRequest) {

            return FollowHttpRedirects(new HttpWebRequestWrapper(httpWebRequest), new HttpWebRequestFactory());

        }

        public static long? GetFileSize(IHttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            long? fileSize = null;

            string originalMethod = httpWebRequest.Method;

            // Attempt to make a HEAD request first. If it fails, we'll fall back to making a GET request.

            foreach (string method in new[] { "HEAD", "GET" }) {

                httpWebRequest.Method = method;

                try {

                    using (WebResponse webResponse = httpWebRequest.GetResponse()) {

                        if (webResponse.Headers.TryGetHeaderValue(HttpResponseHeader.ContentLength, out string contentLength) &&
                            long.TryParse(contentLength, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsedContentLength)) {

                            fileSize = parsedContentLength;

                        }

                        // We'll exit the loop regardless of whether or not we got a content-length header, because trying again with a GET request is unlikely to produce one.

                        break;

                    }

                }
                catch (WebException ex) {

                    // 405 error just indicates that HEAD requests aren't supported by the server, so we shouldn't throw yet (this is a common case).
                    // We'll follow up by attempting a GET request instead.

                    using (WebResponse webResponse = ex.Response)
                        if ((webResponse as IHttpWebResponse)?.StatusCode != HttpStatusCode.MethodNotAllowed)
                            throw ex;

                }

            }

            // Restore original configuration.

            httpWebRequest.Method = originalMethod;

            return fileSize;

        }
        public static long? GetFileSize(HttpWebRequest httpWebRequest) {

            return GetFileSize(new HttpWebRequestWrapper(httpWebRequest));

        }
        public static long? GetFileSize(Uri uri) {

            return GetFileSize(new HttpWebRequestFactory().Create(uri));

        }
        public static long? GetFileSize(string uri) {

            return GetFileSize(new Uri(uri));

        }

    }

}
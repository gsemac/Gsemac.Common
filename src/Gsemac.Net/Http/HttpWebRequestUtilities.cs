using Gsemac.Core;
using Gsemac.Net.Extensions;
using System;
using System.Globalization;
using System.Net;

namespace Gsemac.Net.Http {

    public static class HttpWebRequestUtilities {

        // Public members

        public static HttpStatusCode GetStatusCode(IHttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            HttpStatusCode? statusCode = null;
            WebException lastException = null;

            bool originalAllowAutoRedirect = httpWebRequest.AllowAutoRedirect;
            string originalMethod = httpWebRequest.Method;

            httpWebRequest.AllowAutoRedirect = false;

            // Attempt to make a HEAD request first. If it fails, we'll fall back to making a GET request.

            foreach (string method in new[] { "HEAD", "GET" }) {

                httpWebRequest.Method = method;

                try {

                    using (WebResponse webResponse = httpWebRequest.GetResponse())
                        statusCode = (webResponse as IHttpWebResponse)?.StatusCode;

                }
                catch (WebException ex) {

                    // ex.Response will be a regular HttpWebResponse instead of an IHttpWebResponse when the request fails.

                    using (WebResponse webResponse = ex.Response)
                        statusCode = (webResponse as HttpWebResponse)?.StatusCode;

                    lastException = ex;

                }

                if (statusCode.HasValue && statusCode != HttpStatusCode.MethodNotAllowed)
                    break;

            }

            // Restore original configuration.

            httpWebRequest.AllowAutoRedirect = originalAllowAutoRedirect;
            httpWebRequest.Method = originalMethod;

            if (statusCode.HasValue)
                return statusCode.Value;
            else
                throw lastException ?? new Exception($"Unable to obtain HTTP status code from {httpWebRequest.RequestUri}.");

        }
        public static HttpStatusCode GetStatusCode(HttpWebRequest httpWebRequest) {

            return GetStatusCode(new HttpWebRequestAdapter(httpWebRequest));

        }
        public static HttpStatusCode GetStatusCode(Uri uri) {

            return GetStatusCode(new HttpWebRequestFactory().Create(uri));

        }
        public static HttpStatusCode GetStatusCode(string uri) {

            return GetStatusCode(new Uri(uri));

        }
        public static bool TryGetStatusCode(IHttpWebRequest httpWebRequest, out HttpStatusCode statusCode) {

            try {

                statusCode = GetStatusCode(httpWebRequest);

                return true;

            }
            catch (WebException) {

                statusCode = default;

                return false;

            }

        }
        public static bool TryGetStatusCode(HttpWebRequest httpWebRequest, out HttpStatusCode statusCode) {

            return TryGetStatusCode(new HttpWebRequestAdapter(httpWebRequest), out statusCode);

        }
        public static bool TryGetStatusCode(Uri uri, out HttpStatusCode statusCode) {

            return TryGetStatusCode(new HttpWebRequestFactory().Create(uri), out statusCode);

        }
        public static bool TryGetStatusCode(string uri, out HttpStatusCode statusCode) {

            return TryGetStatusCode(new Uri(uri), out statusCode);

        }

        public static long GetRemoteFileSize(IHttpWebRequest httpWebRequest) {

            if (httpWebRequest is null)
                throw new ArgumentNullException(nameof(httpWebRequest));

            long? fileSize = null;
            WebException lastException = null;

            string originalMethod = httpWebRequest.Method;

            // Attempt to make a HEAD request first. If it fails, we'll fall back to making a GET request.

            foreach (string method in new[] { "HEAD", "GET" }) {

                httpWebRequest.Method = method;

                try {

                    using (WebResponse webResponse = httpWebRequest.GetResponse()) {

                        if (webResponse.Headers.TryGetHeader(HttpResponseHeader.ContentLength, out string contentLength) &&
                            long.TryParse(contentLength, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsedContentLength)) {

                            fileSize = parsedContentLength;

                        }

                        // We'll exit the loop regardless of whether or not we got a content-length header, because trying again with a GET request is unlikely to produce one.

                        break;

                    }

                }
                catch (WebException ex) {

                    lastException = ex;

                    // 405 error just indicates that HEAD requests aren't supported by the server, so we shouldn't throw yet (this is a common case).
                    // We'll follow up by attempting a GET request instead.

                    using (WebResponse webResponse = ex.Response)
                        if ((webResponse as IHttpWebResponse)?.StatusCode != HttpStatusCode.MethodNotAllowed)
                            throw ex;

                }

            }

            // Restore original configuration.

            httpWebRequest.Method = originalMethod;

            if (fileSize.HasValue)
                return fileSize.Value;
            else
                throw lastException ?? new Exception($"Unable to obtain file size from {httpWebRequest.RequestUri}.");

        }
        public static long GetRemoteFileSize(HttpWebRequest httpWebRequest) {

            return GetRemoteFileSize(new HttpWebRequestAdapter(httpWebRequest));

        }
        public static long GetRemoteFileSize(Uri uri) {

            return GetRemoteFileSize(new HttpWebRequestFactory().Create(uri));

        }
        public static long GetRemoteFileSize(string uri) {

            return GetRemoteFileSize(new Uri(uri));

        }
        public static bool TryGetRemoteFileSize(IHttpWebRequest httpWebRequest, out long fileSize) {

            try {

                fileSize = GetRemoteFileSize(httpWebRequest);

                return true;

            }
            catch (WebException) {

                fileSize = default;

                return false;

            }

        }
        public static bool TryGetRemoteFileSize(HttpWebRequest httpWebRequest, out long fileSize) {

            return TryGetRemoteFileSize(new HttpWebRequestAdapter(httpWebRequest), out fileSize);

        }
        public static bool TryGetRemoteFileSize(Uri uri, out long fileSize) {

            return TryGetRemoteFileSize(new HttpWebRequestFactory().Create(uri), out fileSize);

        }
        public static bool TryGetRemoteFileSize(string uri, out long fileSize) {

            return TryGetRemoteFileSize(new Uri(uri), out fileSize);

        }
        public static bool RemoteFileExists(IHttpWebRequest httpWebRequest) {

            // Rather than catching all exceptions, I check for certain conditions that indicate with certainty that the remote file does not exist.
            // For example, if we get a 404 error or the domain name can't be resolved altogether, there's a good chance the file doesn't exist.
            // Gneral connection erorrs are more ambiguous.

            try {

                return GetStatusCode(httpWebRequest) == HttpStatusCode.OK;
            }
            catch (WebException ex) {

                // ex.Response will be a regular HttpWebResponse instead of an IHttpWebResponse when the request fails.

                HttpStatusCode? statusCode = (ex.Response as HttpWebResponse)?.StatusCode;

                switch (ex.Status) {

                    // If the name cannot be resolved, assume the entire endpoint is unavailable/offline.

                    case WebExceptionStatus.NameResolutionFailure:
                        return false;

                    // Check the status code of the response.

                    default:
                        switch (statusCode) {

                            case HttpStatusCode.NotFound:
                                return false;

                            default:
                                throw ex;

                        }
                }

            }

        }
        public static bool RemoteFileExists(HttpWebRequest httpWebRequest) {

            return RemoteFileExists(new HttpWebRequestAdapter(httpWebRequest));

        }
        public static bool RemoteFileExists(Uri uri) {

            return RemoteFileExists(new HttpWebRequestFactory().Create(uri));

        }
        public static bool RemoteFileExists(string uri) {

            return RemoteFileExists(new Uri(uri));

        }

        public static DateTimeOffset GetRemoteDateTime(IHttpWebRequest httpWebRequest) {

            string originalMethod = httpWebRequest.Method;

            try {

                using (WebResponse webResponse = httpWebRequest.GetResponse()) {

                    if (webResponse.Headers.TryGetHeader(HttpResponseHeader.Date, out string date) && DateUtilities.TryParseHttpHeader(date, out DateTimeOffset parsedDate))
                        return parsedDate;

                }

            }
            finally {

                httpWebRequest.Method = originalMethod;

            }

            throw new Exception($"Unable to obtain date from {httpWebRequest.RequestUri}.");

        }
        public static DateTimeOffset GetRemoteDateTime(HttpWebRequest httpWebRequest) {

            return GetRemoteDateTime(new HttpWebRequestAdapter(httpWebRequest));

        }
        public static DateTimeOffset GetRemoteDateTime(Uri uri) {

            return GetRemoteDateTime(new HttpWebRequestFactory().Create(uri));

        }
        public static DateTimeOffset GetRemoteDateTime(string uri) {

            return GetRemoteDateTime(new Uri(uri));

        }
        public static bool TryGetRemoteDateTime(IHttpWebRequest httpWebRequest, out DateTimeOffset date) {

            try {

                date = GetRemoteDateTime(httpWebRequest);

                return true;

            }
            catch (WebException) {

                date = default;

                return false;

            }

        }
        public static bool TryGetRemoteDateTime(HttpWebRequest httpWebRequest, out DateTimeOffset date) {

            return TryGetRemoteDateTime(new HttpWebRequestAdapter(httpWebRequest), out date);

        }
        public static bool TryGetRemoteDateTime(Uri uri, out DateTimeOffset date) {

            return TryGetRemoteDateTime(new HttpWebRequestFactory().Create(uri), out date);

        }
        public static bool TryGetRemoteDateTime(string uri, out DateTimeOffset date) {

            return TryGetRemoteDateTime(new Uri(uri), out date);

        }

        // Internal members

        internal static WebRequest GetInnermostWebRequest(WebRequest webRequest) {

            WebRequest result = webRequest;
            bool exitLoop = false;

            while (!exitLoop && result is object) {

                switch (result) {

                    case HttpWebRequestAdapter httpWebRequestAdapter:
                        result = httpWebRequestAdapter.InnerWebRequest;
                        break;

                    case HttpWebRequestDecoratorBase httpWebRequestDecoratorBase:
                        result = httpWebRequestDecoratorBase.InnerWebRequest;
                        break;

                    default:
                        exitLoop = true;
                        break;

                }

            }

            return result;

        }

    }

}
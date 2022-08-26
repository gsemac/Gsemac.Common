using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.Curl {

    internal abstract class CurlHttpWebResponseBase :
        HttpWebResponseBase {

        // Public members

        public override CookieCollection Cookies {
            get => cookies.GetCookies(ResponseUri);
            set => cookies.Add(value);
        }
        public override Uri ResponseUri => responseUri;

        // Protected members

        protected CurlHttpWebResponseBase(IHttpWebRequest parentRequest, Stream responseStream) :
            this(parentRequest, responseStream, () => null) {
        }
        protected CurlHttpWebResponseBase(IHttpWebRequest parentRequest, Stream responseStream, Func<Exception> exceptionFactory) :
            base(parentRequest.RequestUri, responseStream) {

            this.responseUri = parentRequest.RequestUri;
            this.responseStream = responseStream;
            this.exceptionFactory = exceptionFactory;

            Method = parentRequest.Method;
            ProtocolVersion = parentRequest.ProtocolVersion;

        }

        protected internal void ReadHeadersFromResponseStream() {

            // Read headers from the stream before returning to the caller so our property values are valid.

            ReadHttpHeaders(responseStream);

            // Check status code for success.

            Exception ex = null;

            if (exceptionFactory is object)
                ex = exceptionFactory();

            if (ex is object) {

                // An exception occurred while processing the request.
                // The exception (probably a CurlException) is wrapped in a WebException to match what would be thrown by a regular HttpWebResponse.

                throw new WebException(ex.Message, ex);

            }
            else if (StatusCode == 0) {

                // We didn't read a status code from the stream at all.

                throw new WebException(Properties.ExceptionMessages.ConnectedPartyDidNotRespond, null, WebExceptionStatus.ServerProtocolViolation, this);

            }
            else if (!HttpUtilities.IsSuccessStatusCode(StatusCode)) {

                // We got a response, but didn't get a success code.

                throw new WebException(string.Format(Properties.ExceptionMessages.RemoteServerReturnedAnError, (int)StatusCode, StatusDescription), null, WebExceptionStatus.ProtocolError, this);

            }

        }

        // Private members

        private Uri responseUri;
        private readonly Stream responseStream;
        private readonly CookieContainer cookies = new CookieContainer();
        private readonly Func<Exception> exceptionFactory;

        private void ReadHttpHeaders(Stream responseStream) {

            using (IHttpResponseReader reader = new HttpResponseReader(responseStream)) {

                // Curl will output multiple sets of headers if we are redirected.

                bool readHeaders = true;

                while (readHeaders) {

                    // We might fail to read the status line if we had a redirect that wasn't followed (i.e. "AllowAutoRedirect" is false).
                    // In that case, there are no other responses to read.

                    if (!reader.TryReadStartLine(out IHttpStatusLine statusLine))
                        break;

                    ProtocolVersion = statusLine.ProtocolVersion;
                    StatusCode = statusLine.StatusCode;
                    StatusDescription = statusLine.StatusDescription;

                    Headers.Clear();

                    foreach (IHttpHeader header in reader.Headers)
                        Headers.Add(header.Name, header.Value);

                    // Update response URI and continue reading headers if we were redirected.

                    bool wasRedirected = Headers.TryGetHeader(HttpResponseHeader.Location, out string locationHeader);
                    Uri locationUri = null;

                    readHeaders = wasRedirected || StatusCode == HttpStatusCode.Continue;

                    if (wasRedirected) {

                        if (!Uri.TryCreate(responseUri, locationHeader, out locationUri))
                            throw new WebException(Properties.ExceptionMessages.CannotHandleRedirectFromProtocolToDissimilarOnes, null, WebExceptionStatus.ProtocolError, this);

                    }

                    // Get cookies.

                    if (Headers.TryGetHeader(HttpResponseHeader.SetCookie, out string setCookieHeader))
                        cookies.SetCookies(responseUri, setCookieHeader);

                    // Update the response URI.
                    // This is done after reading the "set-cookie" header so that the cookies are applied to the original domain instead of the new one.

                    if (locationUri is object)
                        responseUri = locationUri;

                }

                // Get charset.

                if (Headers.TryGetHeader(HttpResponseHeader.ContentType, out string contentTypeHeader)) {

                    Match charsetMatch = Regex.Match(contentTypeHeader, @"charset=([^\s]+)");

                    if (charsetMatch.Success)
                        CharacterSet = charsetMatch.Groups[1].Value;

                }

            }

        }

    }

}
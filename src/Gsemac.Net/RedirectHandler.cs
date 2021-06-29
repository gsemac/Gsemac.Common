using Gsemac.Net.Extensions;
using System;
using System.Net;
using System.Threading;

namespace Gsemac.Net {

    // While redirections can be handled automatically be enabling "AllowAutoRedirect", the default implementation ignores the set-cookie header of intermediate responses.
    // This implementation preserves cookies set throughout the entire chain of requests.

    public class RedirectHandler :
        HttpWebRequestHandler {

        // Public members

        public RedirectHandler(IHttpWebRequestFactory httpWebRequestFactory) {

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.httpWebRequestFactory = httpWebRequestFactory;

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // If redirects are not to be handled, simply pass down the request.

            if (!request.AllowAutoRedirect)
                return base.Send(request, cancellationToken);

            int maximumRedirections = request.MaximumAutomaticRedirections;

            request.AllowAutoRedirect = false;

            try {

                IHttpWebResponse response = null;

                for (int redirections = 0; redirections < maximumRedirections; ++redirections) {

                    response = base.Send(request, cancellationToken);

                    if (response is object && IsRedirectStatusCode(response.StatusCode)) {

                        if (response.Headers.TryGetHeader(HttpResponseHeader.Location, out string locationValue) && !string.IsNullOrWhiteSpace(locationValue)) {

                            try {

                                // Follow the redirect to the new location.

                                if (Uri.TryCreate(request.RequestUri, locationValue, out Uri locationUri) && (locationUri.Scheme == Uri.UriSchemeHttp || locationUri.Scheme == Uri.UriSchemeHttps)) {

                                    IHttpWebRequest originatingRequest = request;

                                    // Create a new web request.

                                    // No properties are kept from the original request (including headers) except for the cookies, allowing us to acquire new cookies throughout the redirection.
                                    // Note that if we did copy the headers, we'd also be copying the "Host" header, because it gets set when GetResponse is called in the original request.
                                    // The previous Host value might not be valid for the new endpoint we're redirecting to, which can cause a 404 error.

                                    request = httpWebRequestFactory.Create(locationUri);

                                    request.CookieContainer = originatingRequest.CookieContainer;

                                    // Set the verb for the new request.

                                    // While the standard specifies that the verb is maintained for 302 redirects, browsers have made it a de facto standard that a GET request is used.
                                    // The 307 and 308 status codes were introduced to explicitly instruct the client to use the same verb as the original request.
                                    // https://stackoverflow.com/a/2068504/5383169 (Christopher Orr)

                                    if (response.StatusCode == HttpStatusCode.RedirectKeepVerb || response.StatusCode == (HttpStatusCode)308)
                                        request.Method = originatingRequest.Method;
                                    else
                                        request.Method = "GET";

                                    // Set the referer for new request.
                                    // The referer is omitted when redirecting to a different scheme (e.g. HTTP to HTTPS).

                                    if (locationUri.Scheme == originatingRequest.RequestUri.Scheme) {

                                        // If the hosts are the same, use the full path as the referer.
                                        // However, if the hosts are different, we'll just use the root path as the referer (this is how Chrome handles it).

                                        if (Uri.Compare(originatingRequest.RequestUri, locationUri, UriComponents.Host, UriFormat.Unescaped, StringComparison.OrdinalIgnoreCase) == 0)
                                            request.Referer = originatingRequest.RequestUri.AbsoluteUri;
                                        else
                                            request.Referer = originatingRequest.RequestUri.GetLeftPart(UriPartial.Authority) + "/";

                                    }

                                }
                                else {

                                    // The location URI was malformed.

                                    throw new WebException(Properties.ExceptionMessages.CannotRedirectToDissimilarProtocols, null, WebExceptionStatus.ProtocolError, response as WebResponse);

                                }

                            }
                            finally {

                                response.Close();

                            }

                        }
                        else {

                            // We did not receive a location header even though we were supposed to.

                            response.Close();

                            throw new WebException(string.Format(Properties.ExceptionMessages.TheRemoteServerReturnedAnErrorWithStatusCode, response.StatusCode), null, WebExceptionStatus.ProtocolError, response as WebResponse);

                        }

                    }
                    else {

                        // We did not receive an HTTP redirection.

                        return response;

                    }

                }

                // If we get here, we reached the maximum number of redirections.

                response?.Close();

                throw new WebException(Properties.ExceptionMessages.TooManyAutomaticRedirections, null, WebExceptionStatus.ProtocolError, response as WebResponse);

            }
            finally {

                request.AllowAutoRedirect = true;

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory httpWebRequestFactory;

        private static bool IsRedirectStatusCode(HttpStatusCode statusCode) {

            switch (statusCode) {

                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.Moved:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.RedirectKeepVerb:
                case (HttpStatusCode)308: // PermanentRedirect, doesn't exist in .NET 4.0
                    return true;

                default:
                    return false;

            }

        }

    }

}
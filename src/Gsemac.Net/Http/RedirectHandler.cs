using Gsemac.Net.Extensions;
using Gsemac.Net.Http.Headers;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace Gsemac.Net.Http {

    // While redirections can be handled automatically be enabling "AllowAutoRedirect", the default implementation ignores the set-cookie header of intermediate responses.
    // This implementation preserves cookies set throughout the entire chain of requests.

    public class RedirectHandler :
        HttpWebRequestHandler {

        // Public members

        public TimeSpan MaximumRefreshTimeout { get; set; } = TimeSpan.FromSeconds(5);

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

                    IHttpWebRequest originatingRequest = request;

                    if (HasRedirectStatusCode(response) && response.Headers.TryGet(HttpResponseHeader.Location, out string locationValue) && !string.IsNullOrWhiteSpace(locationValue)) {

                        try {

                            // Follow the redirect to the new location.

                            if (Uri.TryCreate(request.RequestUri, locationValue, out Uri locationUri) && (locationUri.Scheme == Uri.UriSchemeHttp || locationUri.Scheme == Uri.UriSchemeHttps)) {

                                // Create a new web request.

                                // No properties are kept from the original request (including headers) except for the cookies, allowing us to acquire new cookies throughout the redirection.
                                // Note that if we did copy the headers, we'd also be copying the "Host" header, because it gets set when GetResponse is called in the original request.
                                // The previous Host value might not be valid for the new endpoint we're redirecting to, which can cause a 404 error.

                                request = httpWebRequestFactory.Create(locationUri);

                                request.AllowAutoRedirect = false;
                                request.CookieContainer = originatingRequest.CookieContainer;

                                // Do not forward the referer when redirecting to less-secure destinations (HTTPS → HTTP).
                                // https://smerity.com/articles/2013/where_did_all_the_http_referrers_go.html
                                // https://serverfault.com/q/883750

                                if (ShouldForwardRefererHeader(originatingRequest.RequestUri, locationUri))
                                    request.Referer = originatingRequest.Referer;

                                // Set the verb for the new request.

                                // While the standard specifies that the verb is maintained for 302 redirects, browsers have made it a de facto standard that a GET request is used.
                                // The 307 and 308 status codes were introduced to explicitly instruct the client to use the same verb as the original request.
                                // https://stackoverflow.com/a/2068504/5383169 (Christopher Orr)

                                if (response.StatusCode == HttpStatusCode.RedirectKeepVerb || response.StatusCode == (HttpStatusCode)308)
                                    request.Method = originatingRequest.Method;
                                else
                                    request.Method = "GET";

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
                    else if (HasRefreshHeader(response) && RefreshHeaderValue.TryParse(response.Headers["refresh"], out RefreshHeaderValue refreshHeaderValue) && !string.IsNullOrWhiteSpace(refreshHeaderValue.Url) && !refreshHeaderValue.Url.Equals(request.RequestUri.AbsoluteUri)) {

                        // The request had a refresh header.

                        // Refresh headers that either refresh the current page or redirect to the current page are ignored.
                        // This is because some webpages are set to automatically refresh at an interval, which will keep us waiting forever.

                        try {

                            request = CreateRequestFromRefreshHeader(originatingRequest, refreshHeaderValue);

                        }
                        finally {

                            response.Close();

                        }

                    }
                    else {

                        // We either didn't receive a redirection, or we didn't get enough information to perform the redirect.
                        // Note that it is not an error to receive a 3xx status code without a "Location" header-- It's up to the client to decide how to handle that.
                        // An empty "Location" header can be interpreted as a refresh of the current page, but we won't handle that scenario.
                        // https://stackoverflow.com/a/21321976/5383169

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

        private IHttpWebRequest CreateRequestFromRefreshHeader(IHttpWebRequest originatingRequest, RefreshHeaderValue refreshHeaderValue) {

            if (originatingRequest is null)
                throw new ArgumentNullException(nameof(originatingRequest));

            if (refreshHeaderValue is null)
                throw new ArgumentNullException(nameof(refreshHeaderValue));

            // Wait for the timeout period, preferring the request's timeout if it is shorter.

            TimeSpan timeout = new[] {
                                refreshHeaderValue.Timeout,
                                TimeSpan.FromMilliseconds(originatingRequest.Timeout),
                                MaximumRefreshTimeout,
                            }.Min();

            Thread.Sleep(timeout);

            // Initiate a new request to the new endpoint, preserving any cookies that have been set.
            // Unlike regular redirects, no referer is included, and the request is treated like an entirely new GET request. 

            IHttpWebRequest request = httpWebRequestFactory.Create(Url.Combine(originatingRequest.RequestUri.AbsoluteUri, refreshHeaderValue.Url));

            request.CookieContainer = originatingRequest.CookieContainer;

            return request;

        }

        private static bool HasRedirectStatusCode(IHttpWebResponse response) {

            return response is object &&
                HttpUtilities.IsRedirectStatusCode(response.StatusCode);

        }
        private static bool HasRefreshHeader(IHttpWebResponse response) {

            return response is object &&
                response.Headers.TryGet("refresh", out string refreshHeaderValue) &&
                !string.IsNullOrWhiteSpace(refreshHeaderValue);

        }
        private static bool ShouldForwardRefererHeader(Uri sourceUri, Uri destinationUri) {

            // Note that schemes are not case-sensitive.

            // If we're redirecting to the same scheme, it cannot be less secure.

            if (sourceUri.Scheme.Equals(destinationUri.Scheme, StringComparison.OrdinalIgnoreCase))
                return true;

            // If we're redirecting from HTTP → HTTPS, we have a security upgrade.

            if (sourceUri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && destinationUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;

        }

    }

}
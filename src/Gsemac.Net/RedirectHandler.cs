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

                    if (response is object && (response.StatusCode == HttpStatusCode.Ambiguous ||
                        response.StatusCode == HttpStatusCode.Moved ||
                        response.StatusCode == HttpStatusCode.Redirect ||
                        response.StatusCode == HttpStatusCode.RedirectMethod ||
                        response.StatusCode == HttpStatusCode.RedirectKeepVerb)) {

                        if (response.Headers.TryGetHeader(HttpResponseHeader.Location, out string locationValue) && !string.IsNullOrWhiteSpace(locationValue)) {

                            try {

                                // Follow the redirect to the new location.

                                if (Uri.TryCreate(request.RequestUri, locationValue, out Uri locationUri) && (locationUri.Scheme == Uri.UriSchemeHttp || locationUri.Scheme == Uri.UriSchemeHttps)) {

                                    Uri originalUri = request.RequestUri;
                                    string originalMethod = request.Method;

                                    request = httpWebRequestFactory.Create(locationUri)
                                        .WithOptions(HttpWebRequestOptions.FromHttpWebRequest(request));

                                    if (response.StatusCode == HttpStatusCode.RedirectKeepVerb)
                                        request.Method = originalMethod;

                                    // If the host is the same, use the full path as the referer.
                                    // However, if the hosts are different, we'll just use the root path as the referer (this is how Chrome handles it).

                                    if (Uri.Compare(originalUri, locationUri, UriComponents.Host, UriFormat.Unescaped, StringComparison.OrdinalIgnoreCase) == 0)
                                        request.Referer = originalUri.AbsoluteUri;
                                    else
                                        request.Referer = originalUri.GetLeftPart(UriPartial.Authority) + "/";

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

    }

}
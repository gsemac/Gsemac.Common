using Gsemac.Net.Extensions;
using System;
using System.Net;
using System.Threading;

namespace Gsemac.Net {

    // Redirections performed via the "Refresh" header will not automatically be performed by NET's HTTP request implementation.
    // This is a nonstandard header that was introduced by Netscape, but is respected by all major web browsers.

    public class RefreshHandler :
        HttpWebRequestHandler {

        // Public members

        public bool IgnoreTimeout { get; set; } = false;

        public RefreshHandler(IHttpWebRequestFactory httpWebRequestFactory) {

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.httpWebRequestFactory = httpWebRequestFactory;

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // If redirects are not to be handled, simply pass down the request.
            // Note that if RedirectHandler is used, it must be used after RefreshHandler.

            if (!request.AllowAutoRedirect)
                return base.Send(request, cancellationToken);

            int maximumRedirections = request.MaximumAutomaticRedirections;
            TimeSpan requestTimeout = TimeSpan.FromMilliseconds(request.Timeout);
            IHttpWebResponse response = null;

            for (int redirections = 0; redirections < maximumRedirections; ++redirections) {

                response = base.Send(request, cancellationToken);

                if (response.Headers.TryGetHeader("refresh", out string refreshHeaderValue) && !string.IsNullOrWhiteSpace(refreshHeaderValue)) {

                    try {

                        // We encountered a refresh header.

                        RefreshHeader header = new RefreshHeader(refreshHeaderValue);

                        // Wait for the timeout period, preferring the request's timeout if it is shorter.

                        TimeSpan timeout = header.Timeout > requestTimeout ?
                            requestTimeout :
                            header.Timeout;

                        if (!IgnoreTimeout)
                            Thread.Sleep(timeout);

                        // Initiate a new request to the new endpoint, preserving any cookies that have been set.
                        // Unlike with regular redirects, no referer is included, and the request is treated like a new one. 

                        IHttpWebRequest originatingRequest = request;

                        request = httpWebRequestFactory.Create(header.Url);

                        request.CookieContainer = originatingRequest.CookieContainer;

                    }
                    finally {

                        response.Close();

                    }

                }
                else {

                    // We did not encounter a refresh header.

                    return response;

                }

            }


            // If we get here, we reached the maximum number of redirections.

            response?.Close();

            throw new WebException(Properties.ExceptionMessages.TooManyAutomaticRedirections, null, WebExceptionStatus.ProtocolError, response as WebResponse);

        }

        // Private members

        private readonly IHttpWebRequestFactory httpWebRequestFactory;

    }

}
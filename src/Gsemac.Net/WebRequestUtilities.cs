using Gsemac.Net.Extensions;
using System;
using System.Net;

namespace Gsemac.Net {

    public static class WebRequestUtilities {

        // Public members

        public static WebResponse FollowRedirects(IHttpWebRequest httpWebRequest, IHttpWebRequestFactory httpWebRequestFactory) {

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
        public static WebResponse FollowRedirects(HttpWebRequest httpWebRequest) {

            return FollowRedirects(new HttpWebRequestWrapper(httpWebRequest), new HttpWebRequestFactory());

        }

    }

}
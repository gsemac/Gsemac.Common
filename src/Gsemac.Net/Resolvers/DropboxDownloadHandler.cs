using Gsemac.Net.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Gsemac.Net.Resolvers {

    public class DropboxDownloadHandler :
        HttpWebRequestHandler {

        // Public members

        public DropboxDownloadHandler(IHttpWebRequestFactory httpWebRequestFactory) {

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.httpWebRequestFactory = httpWebRequestFactory;

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            string url = request.Address.AbsoluteUri;

            if (url.StartsWith("https://www.dropbox.com/s/")) {

                // We've encountered a share URL.
                // E.g. https://www.dropbox.com/s/hriinb9w3a2107m/iPad%20intro.pdf

                return ResolveDropboxUrl(request, cancellationToken);

            }
            else if (url.StartsWith("https://dl.dropboxusercontent.com/s/")) {

                // We've encounted a "direct" download URL.
                // E.g. https://dl.dropboxusercontent.com/s/hriinb9w3a2107m/iPad%20intro.pdf

                return ResolveDropboxUserContentUrl(request, cancellationToken);

            }
            else {

                // Pass the request through normally.

                return base.Send(request, cancellationToken);

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory httpWebRequestFactory;

        private IHttpWebResponse ResolveDropboxUrl(IHttpWebRequest request, CancellationToken cancellationToken) {

            // Make a GET request to the download page to acquire the necessary cookies to access the download URL.

            IHttpWebResponse response = base.Send(request, cancellationToken);

            response.Close();

            // Make a POST request to "/sharing/fetch_user_content_link" to get a direct download URL.

            Uri fetchUserContentLinkUri = new Uri(new Uri(request.Address.GetLeftPart(UriPartial.Authority)), "/sharing/fetch_user_content_link");

            // We need to pass "t" as a form data field in order to access the file. 

            string t = request.CookieContainer.GetCookies(request.Address)
                .Cast<Cookie>()
                .FirstOrDefault(x => x.Name == "t")?.Value ?? string.Empty;

            // Build the POST request.

            IHttpWebRequest postRequest = httpWebRequestFactory.Create(fetchUserContentLinkUri);

            byte[] postData = new UrlEncodedFormDataBuilder(Encoding.UTF8)
                .WithField("is_xhr", "true")
                .WithField("t", t)
                .WithField("url", request.Address.AbsoluteUri)
                .Build();

            postRequest.Accept = "*/*";
            postRequest.ContentLength = postData.Length;
            postRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            postRequest.CookieContainer = request.CookieContainer;
            postRequest.Method = "POST";
            postRequest.Referer = request.Address.AbsoluteUri;

            postRequest.Headers["x-requested-with"] = "XMLHttpRequest";

            using (Stream stream = postRequest.GetRequestStream())
                stream.Write(postData, 0, postData.Length);

            // Get the final download URL.

            response = base.Send(postRequest, cancellationToken);

            string directDownloadUrl;

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                directDownloadUrl = sr.ReadToEnd();

            response.Close();

            // Finally, send a new request to the direct download URL.

            IHttpWebRequest downloadRequest = httpWebRequestFactory.Create(directDownloadUrl);

            downloadRequest.CookieContainer = request.CookieContainer;

            return base.Send(downloadRequest, cancellationToken);

        }
        private IHttpWebResponse ResolveDropboxUserContentUrl(IHttpWebRequest request, CancellationToken cancellationToken) {

            // We might get redirected to the "speedbump" page before being able to download the file for certain file types.
            // If that happens, we'll need to follow the redirect and then extract the download URL.

            bool requestAllowAutoRedirect = request.AllowAutoRedirect;

            try {

                // Temporarily disable automatic redirection so that we can tell if we're being redirected.

                request.AllowAutoRedirect = false;

                IHttpWebResponse response = base.Send(request, cancellationToken);

                if (HttpUtilities.IsRedirectStatusCode(response.StatusCode) && response.Headers.TryGetHeader(HttpResponseHeader.Location, out string locationHeaderValue)) {

                    // We got redirected, so try to determine the actual download URL.

                    response.Close();

                    IHttpWebRequest redirectRequest = httpWebRequestFactory.Create(locationHeaderValue)
                        .WithOptions(HttpWebRequestOptions.FromHttpWebRequest(request));

                    // Get the final download URL.

                    string speedbumpPageContent;

                    try {

                        response = base.Send(redirectRequest, cancellationToken);

                        using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            speedbumpPageContent = sr.ReadToEnd();

                    }
                    finally {

                        response?.Close();

                    }

                    Match directDownloadUrlMatch = Regex.Match(speedbumpPageContent, @"content_link"": ""([^ ""]+)");

                    // Finally, send a new request to the direct download URL.

                    IHttpWebRequest downloadRequest = httpWebRequestFactory.Create(directDownloadUrlMatch.Groups[1].Value);

                    downloadRequest.CookieContainer = request.CookieContainer;

                    return base.Send(downloadRequest, cancellationToken);

                }
                else {

                    // We didn't get the speedbump page, so just return the response.

                    return response;

                }

            }
            finally {

                request.AllowAutoRedirect = requestAllowAutoRedirect;

            }

        }

    }

}
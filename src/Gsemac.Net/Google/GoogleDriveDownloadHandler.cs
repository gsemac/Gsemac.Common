using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using Gsemac.Polyfills.System;
using Gsemac.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Gsemac.Net.Google {

    public class GoogleDriveDownloadHandler :
        HttpWebRequestHandler {

        // Public members

        public GoogleDriveDownloadHandler(IHttpWebRequestFactory httpWebRequestFactory) {

            if (httpWebRequestFactory is null)
                throw new ArgumentNullException(nameof(httpWebRequestFactory));

            this.httpWebRequestFactory = httpWebRequestFactory;

        }

        // Protected members

        protected override IHttpWebResponse Send(IHttpWebRequest request, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            string url = request.Address.AbsoluteUri;

            if (GetSupportedUrlPrefixes().Any(prefix => url.StartsWith(prefix))) {

                // We've encountered a Google Drive URL.
                // E.g. https://drive.google.com/uc?export=view&id=0B9F7aa5Cm7ZFc3RhcnRlcl9maWxl (direct link)
                // E.g. https://drive.google.com/file/d/0B9F7aa5Cm7ZFc3RhcnRlcl9maWxl/view?usp=sharing (share link)

                return ResolveGoogleDriveUrl(request, string.Empty, cancellationToken);

            }
            else {

                // Pass the request through normally.

                return base.Send(request, cancellationToken);

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory httpWebRequestFactory;

        private IHttpWebResponse ResolveGoogleDriveUrl(IHttpWebRequest request, string confirmationCode, CancellationToken cancellationToken) {

            if (request is null)
                throw new ArgumentNullException(nameof(request));

            Uri requestUri = request.RequestUri;
            string requestUrl = requestUri.AbsoluteUri;
            string fileId = GetFileIdFromUrl(requestUrl);
            string resourceKey = Url.GetQueryParameter(requestUrl, "resourcekey");

            UrlEncodedFormDataBuilder queryStringBuilder = new UrlEncodedFormDataBuilder()
                .WithField("id", fileId);

            if (!string.IsNullOrWhiteSpace(resourceKey))
                queryStringBuilder.WithField("resourcekey", resourceKey);

            if (!string.IsNullOrWhiteSpace(confirmationCode))
                queryStringBuilder.WithField("confirm", confirmationCode);

            string downloadUrl = $"{requestUri.GetLeftPart(UriPartial.Authority)}/uc?export=download&{queryStringBuilder}";

            // If we can't get a direct download URL, send the request through normally.

            if (string.IsNullOrWhiteSpace(fileId))
                return base.Send(request, cancellationToken);

            IHttpWebRequest downloadRequest = httpWebRequestFactory.Create(downloadUrl);

            downloadRequest.AllowAutoRedirect = false;

            IHttpWebResponse downloadResponse = base.Send(downloadRequest, cancellationToken);

            if (downloadResponse.Cookies.Cast<Cookie>().Any(c => c.Name.Contains("download_warning", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrWhiteSpace(confirmationCode)) {

                // Didn't get redirected? We're probably trying to download a file too large for Google to scan for viruses.
                // Ex: https://drive.google.com/uc?export=download&id=0BwmD_VLjROrfTHk4NFg2SndKcjQ

                // We'll need to get the confirmation code in order to proceed with the download, and then repeat this process.

                try {

                    string responseBody;

                    using (Stream responseStream = downloadResponse.GetResponseStream())
                        responseBody = StringUtilities.StreamToString(responseStream);

                    Match confirmationCodeMatch = Regex.Match(responseBody, @"\bconfirm=(.+?)&");

                    if (confirmationCodeMatch.Success) {

                        confirmationCode = confirmationCodeMatch.Groups[1].Value;

                        return ResolveGoogleDriveUrl(downloadRequest, confirmationCode, cancellationToken);

                    }

                }
                finally {

                    downloadResponse?.Close();

                }

            }

            return downloadResponse;

        }

        private static IEnumerable<string> GetSupportedUrlPrefixes() {

            return new[] {
                "https://drive.google.com/uc?",
                "https://drive.google.com/file/",
            };

        }
        private static string GetFileIdFromUrl(string url) {

            Match fileIdMatch = Regex.Match(url, @"\b(?:id=|\/d\/)(.+?)(?=&|\/|$)");

            return fileIdMatch.Groups[1].Value;

        }

    }

}
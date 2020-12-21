using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebDrivers {

    public class FirefoxWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }
        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverInfoCache cache) :
            base(webRequestFactory, cache) {

            this.webRequestFactory = webRequestFactory;

        }

        // Protected members

        protected override IWebDriverInfo GetLatestWebDriverInfo() {

            Uri releasesUri = new Uri("https://github.com/mozilla/geckodriver/releases/latest");

            using (WebClient webClient = new WebClientFactory(webRequestFactory).Create()) {

                // Get download URLs from the latest release.

                string body = webClient.DownloadString(releasesUri);

                Uri downloadUri = Regex.Matches(body, @"<a href=""([^""]+)"" rel=""nofollow""").Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Where(relativeUri => relativeUri.Contains(GetTargetOS()))
                    .Select(relativeUri => new Uri(releasesUri, relativeUri))
                    .FirstOrDefault();

                if (!(downloadUri is null)) {

                    return new WebDriverInfo() {
                        DownloadUri = downloadUri,
                        Version = GetVersion(downloadUri)
                    };

                }

            }

            // We weren't able to get information on the latest web driver.

            return null;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private string GetTargetOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }
        private Version GetVersion(Uri downloadUri) {

            return new Version(Regex.Match(downloadUri.AbsoluteUri, @"v(\d+.\d+.\d+)").Groups[1].Value);

        }

    }

}
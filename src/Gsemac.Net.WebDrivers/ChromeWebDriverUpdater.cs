using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using System;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public class ChromeWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public ChromeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory) :
            base(webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }
        public ChromeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverInfoCache cache) :
            base(webRequestFactory, cache) {

            this.webRequestFactory = webRequestFactory;

        }

        // Protected members

        protected override IWebDriverInfo GetLatestWebDriverInfo() {

            Uri versionUri = new Uri("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");

            // If we can get the current version of Google Chrome, we can select an exact web driver version.

            Version browserVersion = WebBrowserInfo.GetWebBrowserInfo(WebBrowserId.Chrome)?.Version;

            if (!(browserVersion is null))
                versionUri = new Uri(versionUri.AbsoluteUri + $"_{browserVersion.Major}.{browserVersion.Minor}.{browserVersion.Build}");

            using (WebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

                // Get the latest web driver version.

                string latestVersionString = webClient.DownloadString(versionUri);

                // Create a download URL for this version.

                if (!string.IsNullOrWhiteSpace(latestVersionString)) {

                    Uri downloadUri = new Uri(versionUri, $"/{latestVersionString}/chromedriver_{GetTargetOS()}.zip");

                    return new WebDriverInfo() {
                        DownloadUri = downloadUri,
                        Version = new Version(latestVersionString)
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
            // Google only offers a 32-bit build.

            return "win32";

        }

    }

}
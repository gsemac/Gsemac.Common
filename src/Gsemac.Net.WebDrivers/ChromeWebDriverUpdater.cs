using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public class ChromeWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public ChromeWebDriverUpdater() :
            this(WebDriverUpdaterOptions.Default) {
        }
        public ChromeWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(new HttpWebRequestFactory(), webDriverUpdaterOptions) {
        }
        public ChromeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            base(webRequestFactory) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverUpdaterOptions = webDriverUpdaterOptions;

        }

        // Protected members

        protected override string GetWebDriverExecutablePath() {

            if (string.IsNullOrWhiteSpace(webDriverUpdaterOptions.WebDriverDirectoryPath))
                return WebDriverUtilities.ChromeDriverExecutablePath;

            return Path.Combine(webDriverUpdaterOptions.WebDriverDirectoryPath, WebDriverUtilities.ChromeDriverExecutablePath);

        }
        protected override Uri GetWebDriverDownloadUri(IWebBrowserInfo webBrowserInfo) {

            Uri versionUri = new Uri("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");

            // If we can get the current version of Google Chrome, we can select an exact web driver version.

            Version browserVersion = webBrowserInfo?.Version;

            if (browserVersion is object)
                versionUri = new Uri(versionUri.AbsoluteUri + $"_{browserVersion.Major}.{browserVersion.Minor}.{browserVersion.Build}");

            using (WebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

                // Get the latest web driver version.

                string latestVersionString = webClient.DownloadString(versionUri);

                // Create a download URL for this version.

                if (!string.IsNullOrWhiteSpace(latestVersionString)) {

                    Uri downloadUri = new Uri(versionUri, $"/{latestVersionString}/chromedriver_{GetPlatformOS()}.zip");

                    return downloadUri;

                }

            }

            // We weren't able to get information on the latest web driver.

            return null;

        }
        protected override bool IsSupportedWebBrowser(IWebBrowserInfo webBrowserInfo) {

            return webBrowserInfo.Id == WebBrowserId.Chrome;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverUpdaterOptions webDriverUpdaterOptions;

        private string GetPlatformOS() {

            // For now, we'll focus on Windows.
            // Google only offers a 32-bit build.

            return "win32";

        }


    }

}
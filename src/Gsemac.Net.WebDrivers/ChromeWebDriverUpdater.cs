using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using Gsemac.Net.WebBrowsers;
using System;

namespace Gsemac.Net.WebDrivers {

    public class ChromeWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public ChromeWebDriverUpdater() :
            this(Logger.Null) {
        }
        public ChromeWebDriverUpdater(ILogger logger) :
            this(WebDriverUpdaterOptions.Default, logger) {
        }
        public ChromeWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webDriverUpdaterOptions, Logger.Null) {
        }
        public ChromeWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverUpdaterOptions, logger) {
        }
        public ChromeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webRequestFactory, webDriverUpdaterOptions, Logger.Null) {
        }
        public ChromeWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            base(webRequestFactory, new WebDriverUpdaterOptions(webDriverUpdaterOptions) { WebBrowserId = WebBrowserId.GoogleChrome }, logger) {

            this.webRequestFactory = webRequestFactory;

        }

        // Protected members

        protected override Uri GetWebDriverUri(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            Uri versionUri = new Uri("https://chromedriver.storage.googleapis.com/LATEST_RELEASE");

            // If we can get the current version of Google Chrome, we can select an exact web driver version.

            Version browserVersion = webBrowserInfo?.Version;

            if (browserVersion is object)
                versionUri = new Uri(versionUri.AbsoluteUri + $"_{browserVersion.Major}.{browserVersion.Minor}.{browserVersion.Build}");

            using (IWebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

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
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.ChromeDriverExecutablePath;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private string GetPlatformOS() {

            // For now, we'll focus on Windows.
            // Google only offers a 32-bit build.

            return "win32";

        }

    }

}
using Gsemac.IO.Logging;
using Gsemac.Net.GitHub;
using Gsemac.Net.GitHub.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using System;
using System.Linq;

namespace Gsemac.Net.WebDrivers {

    public class FirefoxWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public FirefoxWebDriverUpdater() :
            this(Logger.Null) {
        }
        public FirefoxWebDriverUpdater(ILogger logger) :
            this(WebDriverUpdaterOptions.Default, logger) {
        }
        public FirefoxWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webDriverUpdaterOptions, Logger.Null) {
        }
        public FirefoxWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverUpdaterOptions, logger) {
        }
        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webRequestFactory, webDriverUpdaterOptions, Logger.Null) {
        }
        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            base(webRequestFactory, new WebDriverUpdaterOptions(webDriverUpdaterOptions) { WebBrowserId = BrowserId.Firefox }, logger) {

            this.webRequestFactory = webRequestFactory;

        }

        // Protected members

        protected override Uri GetWebDriverUri(IBrowserInfo webBrowserInfo) {

            string releasesUrl = "https://github.com/mozilla/geckodriver/releases/latest";

            IGitHubClient gitHubClient = new GitHubWebClient(webRequestFactory);
            IRelease release = gitHubClient.GetLatestRelease(releasesUrl);

            IReleaseAsset asset = release.Assets.Where(a => a.Name.Contains(GetPlatformOS()))
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(asset?.DownloadUrl))
                return new Uri(asset.DownloadUrl);

            // We weren't able to get information on the latest web driver.

            return null;

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.GeckoDriverExecutablePath;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private string GetPlatformOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }

    }

}
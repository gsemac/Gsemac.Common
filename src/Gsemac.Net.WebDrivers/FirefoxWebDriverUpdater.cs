using Gsemac.Net.GitHub;
using Gsemac.Net.GitHub.Extensions;
using Gsemac.Net.WebBrowsers;
using System;
using System.Linq;

namespace Gsemac.Net.WebDrivers {

    public class FirefoxWebDriverUpdater :
        WebDriverUpdaterBase {

        // Public members

        public FirefoxWebDriverUpdater() :
            this(WebDriverUpdaterOptions.Default) {
        }
        public FirefoxWebDriverUpdater(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(new HttpWebRequestFactory(), webDriverUpdaterOptions) {
        }
        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            base(WebBrowserId.Firefox, webRequestFactory, webDriverUpdaterOptions) {
        }

        // Protected members

        protected override Uri GetWebDriverUri(IWebBrowserInfo webBrowserInfo, IHttpWebRequestFactory webRequestFactory) {

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

        private string GetPlatformOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }

    }

}
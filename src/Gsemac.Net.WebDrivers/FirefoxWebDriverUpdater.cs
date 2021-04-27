using Gsemac.IO;
using Gsemac.IO.Compression;
using Gsemac.Net.Extensions;
using Gsemac.Net.GitHub;
using Gsemac.Net.GitHub.Extensions;
using Gsemac.Net.WebBrowsers;
using System;
using System.IO;
using System.Linq;
using System.Net;

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
            base(webRequestFactory) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverUpdaterOptions = webDriverUpdaterOptions;

        }

        // Protected members

        protected override string GetWebDriverExecutablePath() {

            if (string.IsNullOrWhiteSpace(webDriverUpdaterOptions.WebDriverDirectoryPath))
                return WebDriverUtilities.GeckoDriverExecutablePath;

            return Path.Combine(webDriverUpdaterOptions.WebDriverDirectoryPath, WebDriverUtilities.GeckoDriverExecutablePath);

        }
        protected override Uri GetWebDriverDownloadUri(IWebBrowserInfo webBrowserInfo) {

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
        protected override bool IsSupportedWebBrowser(IWebBrowserInfo webBrowserInfo) {

            return webBrowserInfo.Id == WebBrowserId.Firefox;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverUpdaterOptions webDriverUpdaterOptions;

        private string GetPlatformOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }

    }

}
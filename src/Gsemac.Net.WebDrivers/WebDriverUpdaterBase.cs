using Gsemac.IO;
using Gsemac.IO.Compression;
using Gsemac.IO.Logging;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.Http.Extensions;
using Gsemac.Net.WebBrowsers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverUpdaterBase :
         IWebDriverUpdater {

        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;

        // Public members

        public IWebDriverInfo Update(IWebBrowserInfo webBrowserInfo, CancellationToken cancellationToken) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (!IsSupportedWebBrowser(webBrowserInfo))
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowserWithBrowserName, webBrowserInfo.Name), nameof(webBrowserInfo));

            logger.Info($"Checking for web driver updates");

            IWebDriverInfo webDriverInfo = GetWebDriverInfo();

            bool updateRequired = (!webDriverInfo.Version?.Equals(webBrowserInfo.Version) ?? true) ||
                !File.Exists(webDriverInfo.ExecutablePath);

            if (updateRequired) {

                logger.Info($"Updating web driver to version {webBrowserInfo.Version}");

                if (DownloadWebDriver(webBrowserInfo, cancellationToken)) {

                    webDriverInfo = new WebDriverInfo() {
                        ExecutablePath = GetWebDriverExecutablePathInternal(),
                        Version = webBrowserInfo.Version
                    };

                    SaveWebDriverInfo(webDriverInfo);

                }

            }
            else
                logger.Info($"Web driver is up to date ({webBrowserInfo.Version})");

            return webDriverInfo;

        }

        // Protected members

        protected WebDriverUpdaterBase() :
            this(Logger.Null) {
        }
        protected WebDriverUpdaterBase(ILogger logger) :
            this(WebDriverUpdaterOptions.Default, logger) {
        }
        protected WebDriverUpdaterBase(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webDriverUpdaterOptions, Logger.Null) {
        }
        protected WebDriverUpdaterBase(IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverUpdaterOptions, logger) {
        }
        protected WebDriverUpdaterBase(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(webRequestFactory, webDriverUpdaterOptions, Logger.Null) {
        }
        protected WebDriverUpdaterBase(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions, ILogger logger) {

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webDriverUpdaterOptions));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.webRequestFactory = webRequestFactory;
            this.webDriverUpdaterOptions = webDriverUpdaterOptions;
            this.logger = new NamedLogger(logger, nameof(WebDriverUpdaterBase));

        }

        protected abstract Uri GetWebDriverUri(IWebBrowserInfo webBrowserInfo);
        protected abstract string GetWebDriverExecutablePath();

        protected void OnDownloadFileProgressChanged(object sender, DownloadFileProgressChangedEventArgs e) {

            DownloadFileProgressChanged?.Invoke(this, e);

        }
        protected void OnDownloadFileCompleted(object sender, DownloadFileCompletedEventArgs e) {

            DownloadFileCompleted?.Invoke(this, e);

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverUpdaterOptions webDriverUpdaterOptions;
        private readonly ILogger logger;

        private string GetWebDriverExecutablePathInternal() {

            string webDriverExecutablePath = GetWebDriverExecutablePath();

            if (!PathUtilities.IsPathRooted(webDriverExecutablePath) && !string.IsNullOrWhiteSpace(webDriverUpdaterOptions.WebDriverDirectoryPath))
                webDriverExecutablePath = Path.Combine(webDriverUpdaterOptions.WebDriverDirectoryPath, webDriverExecutablePath);

            return webDriverExecutablePath;

        }
        private string GetWebDriverInfoFilePath() {

            return PathUtilities.SetFileExtension(GetWebDriverExecutablePathInternal(), ".json");

        }
        private IWebDriverInfo GetWebDriverInfo() {

            string webDriverMetadataFilePath = GetWebDriverInfoFilePath();

            if (File.Exists(webDriverMetadataFilePath)) {

                string metadataJson = File.ReadAllText(webDriverMetadataFilePath);

                return JsonConvert.DeserializeObject<WebDriverInfo>(metadataJson);

            }
            else
                return new WebDriverInfo();

        }
        private void SaveWebDriverInfo(IWebDriverInfo webDriverInfo) {

            string webDriverMetadataFilePath = GetWebDriverInfoFilePath();

            File.WriteAllText(webDriverMetadataFilePath, JsonConvert.SerializeObject(webDriverInfo, Formatting.Indented));

        }

        protected bool IsSupportedWebBrowser(IWebBrowserInfo webBrowserInfo) {

            WebBrowserId webBrowserId = webDriverUpdaterOptions.WebBrowserId;

            return webBrowserId == WebBrowserId.Unknown ||
                webBrowserId.Equals(webBrowserInfo.Id);

        }
        private bool DownloadWebDriver(IWebBrowserInfo webBrowserInfo, CancellationToken cancellationToken) {

            string webDriverExecutablePath = GetWebDriverExecutablePathInternal();

            logger.Info("Getting web driver download url");

            Uri webDriverDownloadUri = GetWebDriverUri(webBrowserInfo);
            string downloadFilePath = PathUtilities.SetFileExtension(Path.GetTempFileName(), ".zip");

            if (webDriverDownloadUri is object) {

                logger.Info($"Downloading {webDriverDownloadUri}");

                using (IWebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

                    webClient.DownloadProgressChanged += (sender, e) => OnDownloadFileProgressChanged(this, new DownloadFileProgressChangedEventArgs(webDriverDownloadUri, downloadFilePath, e));
                    webClient.DownloadFileCompleted += (sender, e) => OnDownloadFileCompleted(this, new DownloadFileCompletedEventArgs(webDriverDownloadUri, downloadFilePath, e.Error is null));

                    webClient.DownloadFileSync(webDriverDownloadUri, downloadFilePath, cancellationToken);

                }

                try {

                    string filePathInArchive = PathUtilities.GetFileName(webDriverExecutablePath);

                    logger.Info($"Extracting {filePathInArchive}");

                    Archive.ExtractFile(downloadFilePath, filePathInArchive, webDriverExecutablePath);

                }
                catch (Exception ex) {

                    logger.Error(ex.ToString());

                    throw ex;

                }
                finally {

                    File.Delete(downloadFilePath);

                }

                // We were able to successfully update the web driver.

                return true;

            }

            // We were not able to successfully update the web driver.

            return false;

        }

    }

}
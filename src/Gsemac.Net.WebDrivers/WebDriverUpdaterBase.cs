using Gsemac.IO;
using Gsemac.IO.Compression;
using Gsemac.IO.Logging;
using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverUpdaterBase :
         IWebDriverUpdater {

        public event LogEventHandler Log;
        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;

        // Public members

        public IWebDriverInfo Update(IWebBrowserInfo webBrowserInfo, CancellationToken cancellationToken) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (!IsSupportedWebBrowser(webBrowserInfo))
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowser, webBrowserInfo.Name), nameof(webBrowserInfo));

            OnLog.Info($"Checking for web driver updates");

            IWebDriverInfo webDriverInfo = GetWebDriverInfo();

            bool updateRequired = (!webDriverInfo.Version?.Equals(webBrowserInfo.Version) ?? true) ||
                !File.Exists(webDriverInfo.ExecutablePath);

            if (updateRequired) {

                OnLog.Info($"Updating web driver to version {webBrowserInfo.Version}");

                if (DownloadWebDriver(webBrowserInfo, cancellationToken)) {

                    webDriverInfo = new WebDriverInfo() {
                        ExecutablePath = GetWebDriverExecutablePathInternal(),
                        Version = webBrowserInfo.Version
                    };

                    SaveWebDriverInfo(webDriverInfo);

                }

            }
            else
                OnLog.Info($"Web driver is up to date ({webBrowserInfo.Version})");

            return webDriverInfo;

        }

        // Protected members

        protected LogEventHandlerWrapper OnLog => new LogEventHandlerWrapper(Log, "Web Driver Updater");

        protected WebDriverUpdaterBase(WebBrowserId webBrowserId, IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) {

            this.webBrowserId = webBrowserId;
            this.webRequestFactory = webRequestFactory;
            this.webDriverUpdaterOptions = webDriverUpdaterOptions;

        }

        protected abstract Uri GetWebDriverUri(IWebBrowserInfo webBrowserInfo, IHttpWebRequestFactory webRequestFactory);
        protected abstract string GetWebDriverExecutablePath();

        protected void OnDownloadFileProgressChanged(object sender, DownloadFileProgressChangedEventArgs e) {

            DownloadFileProgressChanged?.Invoke(this, e);

        }
        protected void OnDownloadFileCompleted(object sender, DownloadFileCompletedEventArgs e) {

            DownloadFileCompleted?.Invoke(this, e);

        }

        // Private members

        private readonly WebBrowserId webBrowserId;
        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverUpdaterOptions webDriverUpdaterOptions;

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

            return webBrowserId == WebBrowserId.Unknown ||
                webBrowserId.Equals(webBrowserInfo.Id);

        }
        private bool DownloadWebDriver(IWebBrowserInfo webBrowserInfo, CancellationToken cancellationToken) {

            string webDriverExecutablePath = GetWebDriverExecutablePathInternal();

            OnLog.Info("Getting web driver download url");

            Uri webDriverDownloadUri = GetWebDriverUri(webBrowserInfo, webRequestFactory);
            string downloadFilePath = PathUtilities.SetFileExtension(Path.GetTempFileName(), ".zip");

            if (webDriverDownloadUri is object) {

                OnLog.Info($"Downloading {webDriverDownloadUri}");

                using (IWebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

                    webClient.DownloadProgressChanged += (sender, e) => OnDownloadFileProgressChanged(this, new DownloadFileProgressChangedEventArgs(webDriverDownloadUri, downloadFilePath, e));
                    webClient.DownloadFileCompleted += (sender, e) => OnDownloadFileCompleted(this, new DownloadFileCompletedEventArgs(webDriverDownloadUri, downloadFilePath, e.Error is null));

                    webClient.DownloadFileSync(webDriverDownloadUri, downloadFilePath, cancellationToken);

                }

                try {

                    string filePathInArchive = PathUtilities.GetFilename(webDriverExecutablePath);

                    OnLog.Info($"Extracting {filePathInArchive}");

                    Archive.ExtractFile(downloadFilePath, filePathInArchive, webDriverExecutablePath);

                }
                catch (Exception ex) {

                    OnLog.Error(ex.ToString());

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
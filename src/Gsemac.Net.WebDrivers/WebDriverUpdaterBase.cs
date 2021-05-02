using Gsemac.IO;
using Gsemac.IO.Compression;
using Gsemac.IO.Logging;
using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverUpdaterBase :
         IWebDriverUpdater {

        public event LogEventHandler Log;
        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;

        // Public members

        public IWebDriverMetadata UpdateWebDriver(IWebBrowserInfo webBrowserInfo) {

            if (!IsSupportedWebBrowser(webBrowserInfo))
                throw new ArgumentException("The given web browser is not valid for this updater.", nameof(webBrowserInfo));

            IWebDriverMetadata webDriverInfo = GetCurrentWebDriverFileInfo();
            bool updateRequired = !webDriverInfo.Version?.Equals(webBrowserInfo.Version) ?? true;

            if (updateRequired) {

                OnLog.Info($"Updating web driver to version {webBrowserInfo.Version}");

                if (DownloadWebDriver(webBrowserInfo)) {

                    webDriverInfo = new WebDriverMetadata() {
                        ExecutablePath = GetWebDriverExecutablePath(),
                        Version = webBrowserInfo.Version
                    };

                    SaveWebDriverInfo(webDriverInfo);

                }

            }
            else
                OnLog.Info("Web driver is up-to-date");

            return webDriverInfo;

        }

        // Protected members

        protected LogEventHelper OnLog => new LogEventHelper("Web Driver Updater", Log);

        protected WebDriverUpdaterBase(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }

        protected abstract string GetWebDriverExecutablePath();
        protected abstract Uri GetWebDriverDownloadUri(IWebBrowserInfo webBrowserInfo);
        protected abstract bool IsSupportedWebBrowser(IWebBrowserInfo webBrowserInfo);

        protected void OnDownloadFileProgressChanged(object sender, DownloadFileProgressChangedEventArgs e) {

            DownloadFileProgressChanged?.Invoke(this, e);

        }
        protected void OnDownloadFileCompleted(object sender, DownloadFileCompletedEventArgs e) {

            DownloadFileCompleted?.Invoke(this, e);

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;

        private string GetWebDriverMetadataFilePath() {

            return PathUtilities.SetFileExtension(GetWebDriverExecutablePath(), ".json");

        }

        private IWebDriverMetadata GetCurrentWebDriverFileInfo() {

            string webDriverMetadataFilePath = GetWebDriverMetadataFilePath();

            if (File.Exists(webDriverMetadataFilePath)) {

                string metadataJson = File.ReadAllText(webDriverMetadataFilePath);

                return JsonConvert.DeserializeObject<WebDriverMetadata>(metadataJson);

            }
            else
                return new WebDriverMetadata();

        }
        private void SaveWebDriverInfo(IWebDriverMetadata webDriverInfo) {

            string webDriverMetadataFilePath = GetWebDriverMetadataFilePath();

            File.WriteAllText(webDriverMetadataFilePath, JsonConvert.SerializeObject(webDriverInfo, Formatting.Indented));

        }
        private bool DownloadWebDriver(IWebBrowserInfo webBrowserInfo) {

            string webDriverExecutablePath = GetWebDriverExecutablePath();

            OnLog.Info("Getting web driver download url");

            Uri webDriverDownloadUri = GetWebDriverDownloadUri(webBrowserInfo);
            string downloadFilePath = PathUtilities.SetFileExtension(Path.GetTempFileName(), ".zip");

            if (webDriverDownloadUri is object) {

                OnLog.Info($"Downloading {webDriverDownloadUri}");

                using (WebClient webClient = webRequestFactory.ToWebClientFactory().Create()) {

                    webClient.DownloadProgressChanged += (sender, e) => OnDownloadFileProgressChanged(this, new DownloadFileProgressChangedEventArgs(webDriverDownloadUri, downloadFilePath, e));
                    webClient.DownloadFileCompleted += (sender, e) => OnDownloadFileCompleted(this, new DownloadFileCompletedEventArgs(webDriverDownloadUri, downloadFilePath, e.Error is null));

                    webClient.DownloadFileSync(webDriverDownloadUri, downloadFilePath);

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
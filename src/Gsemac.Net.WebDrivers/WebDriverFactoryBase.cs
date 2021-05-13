using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverFactoryBase :
        IWebDriverFactory {

        // Public members

        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;
        public event LogEventHandler Log;

        public IWebDriver Create() {

            IWebBrowserInfo webBrowserInfo = webDriverFactoryOptions.DefaultWebBrowser ??
                (webBrowserId != WebBrowserId.Unknown ? WebBrowserInfoFactory.Default.GetInfo(webBrowserId) : WebBrowserInfoFactory.Default.GetDefaultWebBrowser());

            return Create(webBrowserInfo);

        }
        public IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (webBrowserId != WebBrowserId.Unknown && webBrowserInfo.Id != webBrowserId)
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowser, webBrowserInfo.Name), nameof(webBrowserInfo));

            // Get the web driver executable path.

            OnLog.Info($"Creating web driver ({webBrowserInfo})");

            string webDriverExecutablePath = Path.GetFullPath(GetDriverExecutablePathInternal(webBrowserInfo));

            // Create the driver.

            return GetWebDriver(webBrowserInfo, new WebDriverOptions(webDriverOptions) {
                WebDriverExecutablePath = webDriverExecutablePath,
            });

        }

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected LogEventHandlerWrapper OnLog => new LogEventHandlerWrapper(Log, "Web Driver Factory");

        protected WebDriverFactoryBase(WebBrowserId webBrowserId, IHttpWebRequestFactory httpWebRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) {

            this.webBrowserId = webBrowserId;
            this.httpWebRequestFactory = httpWebRequestFactory;
            this.webDriverOptions = webDriverOptions;
            this.webDriverFactoryOptions = webDriverFactoryOptions;

        }

        protected abstract IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions);
        protected abstract string GetWebDriverExecutablePath();

        protected virtual IWebDriverUpdater GetUpdater(IHttpWebRequestFactory httpWebRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) {

            return null;

        }
        protected virtual void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                if (webDriverFactoryOptions.KillWebDriverProcessesOnDispose) {

                    OnLog.Info($"Killing web driver processes");

                    WebDriverUtilities.KillWebDriverProcesses(GetDriverExecutablePathInternal(null));

                }

                isDisposed = true;

            }

        }

        protected void OnDownloadFileProgressChanged(object sender, DownloadFileProgressChangedEventArgs e) {

            DownloadFileProgressChanged?.Invoke(sender, e);

        }
        protected void OnDownloadFileCompleted(object sender, DownloadFileCompletedEventArgs e) {

            DownloadFileCompleted?.Invoke(sender, e);

        }

        // Private members

        private readonly WebBrowserId webBrowserId;
        private readonly IHttpWebRequestFactory httpWebRequestFactory;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private bool isDisposed = false;

        private string GetDriverExecutablePathInternal(IWebBrowserInfo webBrowserInfo) {

            if (!string.IsNullOrWhiteSpace(webDriverOptions.WebDriverExecutablePath))
                return webDriverOptions.WebDriverExecutablePath;

            if (webBrowserInfo is object && webDriverFactoryOptions.AutoUpdateEnabled) {

                IWebDriverUpdater updater = GetUpdaterInternal();

                if (updater is object) {

                    try {

                        IWebDriverInfo webDriverInfo = updater.Update(webBrowserInfo);

                        if (!string.IsNullOrWhiteSpace(webDriverInfo?.ExecutablePath))
                            return webDriverInfo.ExecutablePath;

                    }
                    catch (Exception ex) {

                        OnLog.Error(ex.ToString());

                        if (!webDriverFactoryOptions.IgnoreUpdateErrors)
                            throw ex;

                    }


                }

            }

            if (string.IsNullOrWhiteSpace(webDriverFactoryOptions.WebDriverDirectoryPath))
                return GetWebDriverExecutablePath();

            return Path.Combine(webDriverFactoryOptions.WebDriverDirectoryPath, GetWebDriverExecutablePath());

        }
        private IWebDriverUpdater GetUpdaterInternal() {

            IWebDriverUpdater updater = GetUpdater(httpWebRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectoryPath = webDriverFactoryOptions.WebDriverDirectoryPath
            });

            if (updater is object) {

                updater.Log += OnLog.Log;

                updater.DownloadFileProgressChanged += OnDownloadFileProgressChanged;
                updater.DownloadFileCompleted += OnDownloadFileCompleted;

            }

            return updater;

        }

    }

}
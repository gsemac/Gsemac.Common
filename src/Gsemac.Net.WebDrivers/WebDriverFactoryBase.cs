using Gsemac.IO.Logging;
using Gsemac.IO.Logging.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverFactoryBase :
        IWebDriverFactory {

        // Public members

        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;

        public IWebDriver Create() {

            WebBrowserId webBrowserId = webDriverFactoryOptions.WebBrowserId;

            IWebBrowserInfo webBrowserInfo = webDriverFactoryOptions.DefaultWebBrowserInfo ??
                (webBrowserId != WebBrowserId.Unknown ? WebBrowserInfoFactory.Default.GetInfo(webBrowserId) : WebBrowserInfoFactory.Default.GetDefaultWebBrowser());

            return Create(webBrowserInfo);

        }
        public IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            WebBrowserId webBrowserId = webDriverFactoryOptions.WebBrowserId;

            if (webBrowserId != WebBrowserId.Unknown && webBrowserInfo.Id != webBrowserId)
                throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowserWithBrowserName, webBrowserInfo.Name), nameof(webBrowserInfo));

            // Get the web driver executable path.

            logger.Info($"Creating web driver ({webBrowserInfo})");

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

        protected WebDriverFactoryBase() :
            this(WebDriverOptions.Default) {
        }
        protected WebDriverFactoryBase(ILogger logger) :
         this(WebDriverOptions.Default, logger) {
        }
        protected WebDriverFactoryBase(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        protected WebDriverFactoryBase(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, WebDriverFactoryOptions.Default, logger) {
        }
        protected WebDriverFactoryBase(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        protected WebDriverFactoryBase(IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        protected WebDriverFactoryBase(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        protected WebDriverFactoryBase(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        protected WebDriverFactoryBase(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        protected WebDriverFactoryBase(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) {

            if (webDriverOptions is null)
                throw new ArgumentNullException(nameof(webDriverOptions));

            if (webDriverFactoryOptions is null)
                throw new ArgumentNullException(nameof(webDriverFactoryOptions));

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.webRequestFactory = webRequestFactory;
            this.webDriverOptions = webDriverOptions;
            this.webDriverFactoryOptions = webDriverFactoryOptions;
            this.logger = new NamedLogger(logger, nameof(WebDriverFactoryBase));

        }

        protected abstract IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions);
        protected abstract string GetWebDriverExecutablePath();
        protected virtual IWebDriverUpdater GetUpdater() {

            return null;

        }

        protected virtual void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                updaterCancellationTokenSource.Cancel();
                updaterCancellationTokenSource.Dispose();

                if (webDriverFactoryOptions.KillWebDriverProcessesOnDispose) {

                    logger.Info($"Killing web driver processes");

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

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly ILogger logger;
        private readonly CancellationTokenSource updaterCancellationTokenSource = new CancellationTokenSource();
        private bool isDisposed = false;

        private string GetDriverExecutablePathInternal(IWebBrowserInfo webBrowserInfo) {

            if (!string.IsNullOrWhiteSpace(webDriverOptions.WebDriverExecutablePath))
                return webDriverOptions.WebDriverExecutablePath;

            if (webBrowserInfo is object && webDriverFactoryOptions.AutoUpdateEnabled) {

                IWebDriverUpdater updater = GetUpdaterInternal();

                if (updater is object) {

                    try {

                        IWebDriverInfo webDriverInfo = updater.Update(webBrowserInfo, updaterCancellationTokenSource.Token);

                        if (!string.IsNullOrWhiteSpace(webDriverInfo?.ExecutablePath))
                            return webDriverInfo.ExecutablePath;

                    }
                    catch (Exception ex) {

                        logger.Error(ex.ToString());

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

            IWebDriverUpdater updater = GetUpdater();

            if (updater is object) {

                updater.DownloadFileProgressChanged += OnDownloadFileProgressChanged;
                updater.DownloadFileCompleted += OnDownloadFileCompleted;

            }

            return updater;

        }

    }

}
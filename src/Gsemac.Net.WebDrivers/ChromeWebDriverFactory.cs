using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public sealed class ChromeWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public ChromeWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
           this(new HttpWebRequestFactory(), webDriverOptions, webDriverFactoryOptions) {
        }
        public ChromeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverOptions = webDriverOptions;
            this.webDriverFactoryOptions = webDriverFactoryOptions;

        }

        public override IWebDriver Create() {

            return Create(webDriverFactoryOptions.DefaultWebBrowser ?? WebBrowserInfo.GetWebBrowserInfo(WebBrowserId.Chrome));

        }
        public override IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo.Id != WebBrowserId.Chrome)
                throw new ArgumentException("The given web browser is not valid for this factory.", nameof(webBrowserInfo));

            // Get the web driver executable path.

            OnLog.Info($"Creating web driver ({webBrowserInfo})");

            string webDriverExecutablePath = Path.GetFullPath(GetWebDriverExecutablePath(webBrowserInfo));
            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverExecutablePath);

            // Create the driver service.

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = webBrowserInfo.ExecutablePath
            };

            ConfigureDriverOptions(driverOptions);

            ChromeDriver driver = new ChromeDriver(driverService, driverOptions);

            ConfigureDriver(driver);

            return driver;

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                if (webDriverFactoryOptions.KillWebDriverProcessesOnDispose) {

                    OnLog.Info($"Killing web driver processes");

                    WebDriverUtilities.KillWebDriverProcesses(GetWebDriverExecutablePath(null));

                }

                isDisposed = true;

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private bool isDisposed = false;

        private string GetWebDriverExecutablePath(IWebBrowserInfo webBrowserInfo) {

            if (!string.IsNullOrWhiteSpace(webDriverOptions.WebDriverExecutablePath))
                return webDriverOptions.WebDriverExecutablePath;

            if (webBrowserInfo is object && webDriverFactoryOptions.AutoUpdateEnabled) {

                OnLog.Info($"Checking for web driver updates");

                IWebDriverVersionInfo webDriverInfo = GetWebDriverUpdater().UpdateWebDriver(webBrowserInfo);

                if (!string.IsNullOrWhiteSpace(webDriverInfo?.ExecutablePath))
                    return webDriverInfo.ExecutablePath;

            }

            if (string.IsNullOrWhiteSpace(webDriverFactoryOptions.WebDriverDirectory))
                return WebDriverUtilities.ChromeDriverExecutablePath;

            return Path.Combine(webDriverFactoryOptions.WebDriverDirectory, WebDriverUtilities.ChromeDriverExecutablePath);

        }
        private IWebDriverUpdater GetWebDriverUpdater() {

            IWebDriverUpdater updater = new ChromeWebDriverUpdater(webRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectory = webDriverFactoryOptions.WebDriverDirectory
            });

            updater.Log += (sender, e) => OnLog.OnLog(e.Message);

            updater.DownloadFileProgressChanged += OnDownloadFileProgressChanged;
            updater.DownloadFileCompleted += OnDownloadFileCompleted;

            return updater;

        }

        private void ConfigureDriverService(ChromeDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        private void ConfigureDriverOptions(ChromeOptions options) {

            if (webDriverOptions.Headless)
                options.AddArgument("--headless");

            if (!string.IsNullOrEmpty(webDriverOptions.UserAgent))
                options.AddArgument($"--user-agent={webDriverOptions.UserAgent}");

            if (!webDriverOptions.Proxy.IsEmpty())
                options.AddArgument($"--proxy-server={webDriverOptions.Proxy.ToProxyString()}");

            options.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)webDriverOptions.PageLoadStrategy;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            options.AddArgument($"--window-size={webDriverOptions.WindowSize.Width},{webDriverOptions.WindowSize.Height}");
            options.AddArgument($"--window-position={webDriverOptions.WindowPosition.X},{webDriverOptions.WindowPosition.Y}");

            // Disable the "navigator.webdriver" property.

            options.AddArgument("--disable-blink-features=AutomationControlled");

        }
        private void ConfigureDriver(ChromeDriver driver) {

            if (webDriverOptions.Stealth) {

                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.utils);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.chrome_app);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.chrome_runtime);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.iframe_contentWindow);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.media_codecs);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_languages);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_permissions);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_plugins);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_vendor);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_webdriver);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.webgl_vendor);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.window_outerdimensions);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_hardwareConcurrency);
                //driver.AddScriptToEvaluateOnNewDocument("(() => { utils = undefined; })();");

            }

            driver.Manage().Window.Position = webDriverOptions.WindowPosition;

        }

    }

}
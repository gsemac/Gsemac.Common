using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    class FirefoxWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public FirefoxWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
           this(new HttpWebRequestFactory(), webDriverOptions, webDriverFactoryOptions) {
        }
        public FirefoxWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverOptions = webDriverOptions;
            this.webDriverFactoryOptions = webDriverFactoryOptions;

        }

        public override IWebDriver Create() {

            return Create(WebBrowserInfo.GetWebBrowserInfo(WebBrowserId.Firefox));

        }
        public override IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (webBrowserInfo.Id != WebBrowserId.Firefox)
                throw new ArgumentException("The given web browser is not valid for this factory.", nameof(webBrowserInfo));

            // Get the web driver executable path.

            OnLog.Info($"Creating web driver ({webBrowserInfo})");

            string webDriverExecutablePath = Path.GetFullPath(GetWebDriverExecutablePath(webBrowserInfo));
            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverExecutablePath);

            // Create the driver service.

            FirefoxDriverService driverService = FirefoxDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = webBrowserInfo.ExecutablePath
            };

            ConfigureDriverOptions(driverOptions);

            FirefoxDriver driver = new FirefoxDriver(driverService, driverOptions);

            ConfigureDriver(driver);

            return driver;

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;

        private string GetWebDriverExecutablePath(IWebBrowserInfo webBrowserInfo) {

            if (!string.IsNullOrWhiteSpace(webDriverOptions.WebDriverExecutablePath))
                return webDriverOptions.WebDriverExecutablePath;

            if (webDriverFactoryOptions.AutoUpdateEnabled) {

                OnLog.Info($"Checking for web driver updates");

                IWebDriverInfo webDriverInfo = GetWebDriverUpdater().GetWebDriver(webBrowserInfo);

                if (!string.IsNullOrWhiteSpace(webDriverInfo?.ExecutablePath))
                    return webDriverInfo.ExecutablePath;

            }

            if (string.IsNullOrWhiteSpace(webDriverFactoryOptions.WebDriverDirectory))
                return WebDriverUtilities.GeckoDriverExecutablePath;

            return Path.Combine(webDriverFactoryOptions.WebDriverDirectory, WebDriverUtilities.GeckoDriverExecutablePath);

        }
        private IWebDriverUpdater GetWebDriverUpdater() {

            IWebDriverUpdater updater = new FirefoxWebDriverUpdater(webRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectory = webDriverFactoryOptions.WebDriverDirectory
            });

            updater.Log += (sender, e) => OnLog.OnLog(e.Message);

            return updater;

        }

        private void ConfigureDriverService(FirefoxDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        private void ConfigureDriverOptions(FirefoxOptions options) {

            if (webDriverOptions.Headless)
                options.AddArgument("--headless");

            // Apply user agent.

            FirefoxProfile profile = new FirefoxProfile {
                DeleteAfterUse = true
            };

            if (!string.IsNullOrEmpty(webDriverOptions.UserAgent))
                profile.SetPreference("general.useragent.override", webDriverOptions.UserAgent);

            // If the user specified a proxy, apply the proxy.

            if (!webDriverOptions.Proxy.IsEmpty()) {

                string proxyAbsoluteUri = webDriverOptions.Proxy.ToProxyString();

                Proxy proxy = new Proxy {
                    HttpProxy = proxyAbsoluteUri,
                    SslProxy = proxyAbsoluteUri
                };

                options.Proxy = proxy;

            }

            if (webDriverOptions.DisablePopUps)
                profile.SetPreference("dom.popup_allowed_events", "");

            options.Profile = profile;

            options.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)webDriverOptions.PageLoadStrategy;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            options.AddArguments($"--width={webDriverOptions.WindowSize.Width}");
            options.AddArguments($"--height={webDriverOptions.WindowSize.Height}");

            // Disable the "navigator.webdriver" property.

            profile.SetPreference("dom.webdriver.enabled", false);

        }
        private void ConfigureDriver(FirefoxDriver driver) {

            driver.Manage().Window.Position = webDriverOptions.WindowPosition;

        }

    }

}
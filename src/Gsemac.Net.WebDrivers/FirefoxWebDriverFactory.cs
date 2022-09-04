using Gsemac.IO.Logging;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    class FirefoxWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public FirefoxWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public FirefoxWebDriverFactory(ILogger logger) :
         this(WebDriverOptions.Default, logger) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, WebDriverFactoryOptions.Default, logger) {
        }
        public FirefoxWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public FirefoxWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        public FirefoxWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        public FirefoxWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        public FirefoxWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            base(webRequestFactory, webDriverOptions, new WebDriverFactoryOptions(webDriverFactoryOptions) { WebBrowserId = WebBrowserId.Firefox }, logger) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverFactoryOptions = webDriverFactoryOptions;
            this.logger = new NamedLogger(logger, nameof(FirefoxWebDriverFactory));

        }

        // Protected members

        protected override IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverOptions.WebDriverExecutablePath);

            FirefoxDriverService driverService = FirefoxDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = webBrowserInfo.ExecutablePath
            };

            ConfigureDriverOptions(driverOptions, webDriverOptions);

            FirefoxDriver driver = new FirefoxDriver(driverService, driverOptions);

            ConfigureDriver(driver, webDriverOptions);

            return driver;

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.GeckoDriverExecutablePath;

        }
        protected override IWebDriverUpdater GetUpdater() {

            return new FirefoxWebDriverUpdater(webRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectoryPath = webDriverFactoryOptions.WebDriverDirectoryPath,
            });

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly ILogger logger;

        private void ConfigureDriverService(FirefoxDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        private void ConfigureDriverOptions(FirefoxOptions options, IWebDriverOptions webDriverOptions) {

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

            if (webDriverOptions.Stealth)
                profile.SetPreference("dom.webdriver.enabled", false);

        }
        private void ConfigureDriver(FirefoxDriver driver, IWebDriverOptions webDriverOptions) {

            driver.Manage().Window.Position = webDriverOptions.WindowPosition;



        }

    }

}
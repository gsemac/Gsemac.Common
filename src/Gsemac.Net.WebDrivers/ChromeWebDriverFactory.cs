using Gsemac.IO.Logging;
using Gsemac.Net.Extensions;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public sealed class ChromeWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public ChromeWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public ChromeWebDriverFactory(ILogger logger) :
         this(WebDriverOptions.Default, logger) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, WebDriverFactoryOptions.Default, logger) {
        }
        public ChromeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public ChromeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        public ChromeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        public ChromeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            base(webRequestFactory, webDriverOptions, new WebDriverFactoryOptions(webDriverFactoryOptions) { WebBrowserId = WebBrowserId.Chrome }, logger) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverFactoryOptions = webDriverFactoryOptions;
            this.logger = new NamedLogger(logger, nameof(ChromeWebDriverFactory));

        }

        // Protected members

        protected override IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            return GetWebDriverInternal(webBrowserInfo, webDriverOptions, overriddenUserAgent: string.Empty);

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.ChromeDriverExecutablePath;

        }
        protected override IWebDriverUpdater GetUpdater() {

            return new ChromeWebDriverUpdater(webRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectoryPath = webDriverFactoryOptions.WebDriverDirectoryPath,
            });

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly ILogger logger;

        private IWebDriver GetWebDriverInternal(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions, string overriddenUserAgent) {

            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverOptions.WebDriverExecutablePath);

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = webBrowserInfo.ExecutablePath
            };

            ConfigureDriverOptions(driverOptions, webDriverOptions, overriddenUserAgent);

            IWebDriver driver = new ChromeDriver(driverService, driverOptions);

            ConfigureDriver(driver as ChromeDriver, webDriverOptions);

            if (string.IsNullOrEmpty(overriddenUserAgent))
                driver = OverrideHeadlessUserAgent(driver, webBrowserInfo, webDriverOptions);

            return driver;

        }

        private void ConfigureDriverService(ChromeDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        private void ConfigureDriverOptions(ChromeOptions options, IWebDriverOptions webDriverOptions, string overriddenUserAgent) {

            if (webDriverOptions.Headless)
                options.AddArgument("--headless");

            string userAgent = !string.IsNullOrEmpty(webDriverOptions.UserAgent) ?
                webDriverOptions.UserAgent :
                overriddenUserAgent;

            if (!string.IsNullOrEmpty(userAgent))
                options.AddArgument($"--user-agent={userAgent}");

            if (!webDriverOptions.Proxy.IsEmpty())
                options.AddArgument($"--proxy-server={webDriverOptions.Proxy.ToProxyString()}");

            options.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)webDriverOptions.PageLoadStrategy;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            options.AddArgument($"--window-size={webDriverOptions.WindowSize.Width},{webDriverOptions.WindowSize.Height}");
            options.AddArgument($"--window-position={webDriverOptions.WindowPosition.X},{webDriverOptions.WindowPosition.Y}");

            // Disable the "navigator.webdriver" property.

            if (webDriverOptions.Stealth)
                options.AddArgument("--disable-blink-features=AutomationControlled");

        }
        private void ConfigureDriver(ChromeDriver driver, IWebDriverOptions webDriverOptions) {

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

        private IWebDriver OverrideHeadlessUserAgent(IWebDriver webDriver, IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            if (webDriverOptions.Stealth && webDriverOptions.Headless && string.IsNullOrWhiteSpace(webDriverOptions.UserAgent)) {

                string userAgent = webDriver.GetUserAgent();

                // The user agent will contain the string "HeadlessChrome" when using headless mode.

                if (userAgent.Contains("HeadlessChrome/")) {

                    logger.Info("HeadlessChrome detected; patching user agent");

                    string newUserAgent = userAgent.Replace("HeadlessChrome/", "Chrome/");

                    // Recreate the web driver using the new user agent string.

                    webDriver.Quit();
                    webDriver.Dispose();

                    return GetWebDriverInternal(webBrowserInfo, webDriverOptions, newUserAgent);

                }

            }

            // Return the web driver unmodified.

            return webDriver;

        }

    }

}
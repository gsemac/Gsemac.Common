using Gsemac.Net.Extensions;
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
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public ChromeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public ChromeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(new HttpWebRequestFactory(), webDriverOptions, webDriverFactoryOptions) {
        }
        public ChromeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            base(WebBrowserId.Chrome, webRequestFactory, webDriverOptions, webDriverFactoryOptions) {
        }

        // Protected members

        protected override IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverOptions.WebDriverExecutablePath);

            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = webBrowserInfo.ExecutablePath
            };

            ConfigureDriverOptions(driverOptions, webDriverOptions);

            ChromeDriver driver = new ChromeDriver(driverService, driverOptions);

            ConfigureDriver(driver, webDriverOptions);

            return driver;

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.ChromeDriverExecutablePath;

        }
        protected override IWebDriverUpdater GetUpdater(IHttpWebRequestFactory httpWebRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) {

            return new ChromeWebDriverUpdater(httpWebRequestFactory, webDriverUpdaterOptions);

        }

        // Private members

        private void ConfigureDriverService(ChromeDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        private void ConfigureDriverOptions(ChromeOptions options, IWebDriverOptions webDriverOptions) {

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

    }

}
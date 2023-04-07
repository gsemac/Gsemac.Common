using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public sealed class EdgeWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public EdgeWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public EdgeWebDriverFactory(ILogger logger) :
         this(WebDriverOptions.Default, logger) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, WebDriverFactoryOptions.Default, logger) {
        }
        public EdgeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public EdgeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        public EdgeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        public EdgeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            base(webRequestFactory, webDriverOptions, new WebDriverFactoryOptions(webDriverFactoryOptions) { WebBrowserId = BrowserId.Edge }, logger) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverFactoryOptions = webDriverFactoryOptions;
            this.logger = new NamedLogger(logger, nameof(EdgeWebDriverFactory));

        }

        // Protected members

        protected override IWebDriver GetWebDriver(IBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            string webDriverDirectoryPath = Path.GetDirectoryName(webDriverOptions.WebDriverExecutablePath);

            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService(webDriverDirectoryPath);

            ConfigureDriverService(driverService);

            EdgeOptions driverOptions = new EdgeOptions();

            //ConfigureDriverOptions(driverOptions);

            EdgeDriver driver = new EdgeDriver(driverService, driverOptions);

            //ConfigureDriver(driver);

            return driver;

        }
        protected override string GetWebDriverExecutablePath() {

            return WebDriverUtilities.EdgeDriverExecutablePath;

        }
        protected override IWebDriverUpdater GetUpdater() {

            return new EdgeWebDriverUpdater(webRequestFactory, new WebDriverUpdaterOptions() {
                WebDriverDirectoryPath = webDriverFactoryOptions.WebDriverDirectoryPath,
            });

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly ILogger logger;

        private void ConfigureDriverService(EdgeDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        //private void ConfigureDriverOptions(EdgeOptions options) {}
        //private void ConfigureDriver(EdgeDriver driver) {}


    }

}
using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public class EdgeWebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public EdgeWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public EdgeWebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public EdgeWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(new HttpWebRequestFactory(), webDriverOptions, webDriverFactoryOptions) {
        }
        public EdgeWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            base(WebBrowserId.Edge, webRequestFactory, webDriverOptions, webDriverFactoryOptions) {
        }

        // Protected members

        protected override IWebDriver GetWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

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
        protected override IWebDriverUpdater GetUpdater(IHttpWebRequestFactory httpWebRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) {

            return new EdgeWebDriverUpdater(httpWebRequestFactory, webDriverUpdaterOptions);

        }

        // Private members

        private void ConfigureDriverService(EdgeDriverService service) {

            service.HideCommandPromptWindow = true;

        }
        //private void ConfigureDriverOptions(EdgeOptions options) {}
        //private void ConfigureDriver(EdgeDriver driver) {}


    }

}
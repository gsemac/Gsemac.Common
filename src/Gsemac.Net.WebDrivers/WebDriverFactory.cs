using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public sealed class WebDriverFactory :
         WebDriverFactoryBase {

        // Public members

        public WebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) {

            if (webDriverOptions is null)
                throw new ArgumentNullException(nameof(webDriverOptions));

            if (webDriverFactoryOptions is null)
                throw new ArgumentNullException(nameof(webDriverFactoryOptions));

            this.webDriverOptions = webDriverOptions;
            this.webDriverFactoryOptions = webDriverFactoryOptions;

        }

        public override IWebDriver Create() {

            return Create(WebBrowserInfo.GetDefaultWebBrowserInfo());

        }
        public override IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            IWebDriverFactory factory = null;

            switch (webBrowserInfo.Id) {

                case WebBrowserId.Chrome:
                    factory = new ChromeWebDriverFactory(webDriverOptions, webDriverFactoryOptions);
                    break;

                case WebBrowserId.Firefox:
                    factory = new FirefoxWebDriverFactory(webDriverOptions, webDriverFactoryOptions);
                    break;

            }

            if (factory is null)
                throw new ArgumentException("The given web browser is not supported.");

            using (factory) {

                factory.Log += (sender, e) => OnLog.OnLog(e.Message);

                return factory.Create(webBrowserInfo);

            }

        }

        // Private members

        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;

    }

}
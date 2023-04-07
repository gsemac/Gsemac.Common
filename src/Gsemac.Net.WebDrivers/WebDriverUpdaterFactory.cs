using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using System;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverUpdaterFactory :
        IWebDriverUpdaterFactory {

        // Public members

        public WebDriverUpdaterFactory() :
            this(WebDriverUpdaterOptions.Default) {
        }
        public WebDriverUpdaterFactory(IWebDriverUpdaterOptions webDriverUpdaterOptions) :
            this(new HttpWebRequestFactory(), webDriverUpdaterOptions) {
        }
        public WebDriverUpdaterFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverUpdaterOptions webDriverUpdaterOptions) {

            this.webRequestFactory = webRequestFactory;
            this.webDriverUpdaterOptions = webDriverUpdaterOptions;

        }

        public IWebDriverUpdater Create(IBrowserInfo webBrowserInfo) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            switch (webBrowserInfo.Id) {

                case BrowserId.Chrome:
                    return new ChromeWebDriverUpdater(webRequestFactory, webDriverUpdaterOptions);

                case BrowserId.Edge:
                    return new EdgeWebDriverUpdater(webRequestFactory, webDriverUpdaterOptions);

                case BrowserId.Firefox:
                    return new FirefoxWebDriverUpdater(webRequestFactory, webDriverUpdaterOptions);

                default:
                    throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowserWithBrowserName, webBrowserInfo.Name), nameof(webBrowserInfo));

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverUpdaterOptions webDriverUpdaterOptions;

    }

}
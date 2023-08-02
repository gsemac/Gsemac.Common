using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Gsemac.Net.WebDrivers {

    public sealed class WebDriverFactory :
         IWebDriverFactory {

        // Public members

        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;
        public event DownloadFileCompletedEventHandler DownloadFileCompleted;

        public static WebDriverFactory Default => new WebDriverFactory();

        public WebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public WebDriverFactory(ILogger logger) :
            this(WebDriverOptions.Default, logger) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, WebDriverFactoryOptions.Default) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, WebDriverFactoryOptions.Default, logger) {
        }
        public WebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public WebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        public WebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        public WebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        public WebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) {

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
            this.logger = new NamedLogger(logger, nameof(WebDriverFactory));

        }

        public IWebDriver Create() {

            return Create(webDriverFactoryOptions.WebBrowser ?? WebBrowserInfoFactory.Default.GetDefaultWebBrowser());

        }
        public IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebDriverFactory));

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            return GetOrCreateFactory(webBrowserInfo).Create(webBrowserInfo);

        }

        public void Dispose() {

            if (!isDisposed) {

                isDisposed = true;

                foreach (IWebDriverFactory factory in factoryDict.Values)
                    factory.Dispose();

            }

        }

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly ILogger logger;
        private readonly IDictionary<WebBrowserId, IWebDriverFactory> factoryDict = new Dictionary<WebBrowserId, IWebDriverFactory>();
        private bool isDisposed = false;

        private IWebDriverFactory GetOrCreateFactory(IWebBrowserInfo webBrowserInfo) {

            IWebDriverFactory factory = null;

            lock (factoryDict) {

                if (!factoryDict.TryGetValue(webBrowserInfo.Id, out factory)) {

                    switch (webBrowserInfo.Id) {

                        case WebBrowserId.Chrome:
                            factoryDict[webBrowserInfo.Id] = new ChromeWebDriverFactory(webRequestFactory, webDriverOptions, webDriverFactoryOptions, logger);
                            break;

                        case WebBrowserId.Edge:
                            factoryDict[webBrowserInfo.Id] = new EdgeWebDriverFactory(webRequestFactory, webDriverOptions, webDriverFactoryOptions, logger);
                            break;

                        case WebBrowserId.Firefox:
                            factoryDict[webBrowserInfo.Id] = new FirefoxWebDriverFactory(webRequestFactory, webDriverOptions, webDriverFactoryOptions, logger);
                            break;

                        default:
                            throw new ArgumentException(string.Format(Properties.ExceptionMessages.UnsupportedWebBrowserWithBrowserName, webBrowserInfo.Name), nameof(webBrowserInfo));

                    }

                    factory = factoryDict[webBrowserInfo.Id];

                    factory.DownloadFileCompleted += DownloadFileCompleted;
                    factory.DownloadFileProgressChanged += DownloadFileProgressChanged;

                }

            }

            return factory;

        }


    }

}
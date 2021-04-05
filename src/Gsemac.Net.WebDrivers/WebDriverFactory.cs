using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

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
        public WebDriverFactory(IWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
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

            return Create(webDriverFactoryOptions.DefaultWebBrowser ?? WebBrowserInfo.GetDefaultWebBrowserInfo());

        }
        public override IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebDriverFactory));

            return GetOrCreateFactory(webBrowserInfo.Id).Create(webBrowserInfo);

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (!isDisposed && disposing) {

                isDisposed = true;

                foreach (IWebDriverFactory factory in factoryDict.Values)
                    factory.Dispose();

            }

            base.Dispose(disposing);

        }

        // Private members

        private readonly IWebDriverOptions webDriverOptions;
        private readonly IWebDriverFactoryOptions webDriverFactoryOptions;
        private readonly IDictionary<WebBrowserId, IWebDriverFactory> factoryDict = new Dictionary<WebBrowserId, IWebDriverFactory>();
        private readonly object factoryDictLock = new object();
        private bool isDisposed = false;

        private IWebDriverFactory GetOrCreateFactory(WebBrowserId webBrowserId) {

            IWebDriverFactory factory = null;

            lock (factoryDictLock) {

                if (!factoryDict.TryGetValue(webBrowserId, out factory)) {

                    switch (webBrowserId) {

                        case WebBrowserId.Chrome:
                            factoryDict[webBrowserId] = new ChromeWebDriverFactory(webDriverOptions, webDriverFactoryOptions);
                            break;

                        case WebBrowserId.Firefox:
                            factoryDict[webBrowserId] = new FirefoxWebDriverFactory(webDriverOptions, webDriverFactoryOptions);
                            break;

                        default:
                            throw new ArgumentException("The given web browser is not supported.");

                    }

                    factory = factoryDict[webBrowserId];

                    factory.Log += OnLog.Log;

                    factory.DownloadFileCompleted += OnDownloadFileCompleted;
                    factory.DownloadFileProgressChanged += OnDownloadFileProgressChanged;

                }

            }

            return factory;

        }

    }

}
using Gsemac.IO.Logging;
using Gsemac.Net.Http;
using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using Gsemac.Net.WebDrivers.Properties;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public sealed class PooledWebDriverFactory :
        IWebDriverFactory {

        // Public members

        public event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged {
            add => baseFactory.DownloadFileProgressChanged += value;
            remove => baseFactory.DownloadFileProgressChanged -= value;
        }
        public event DownloadFileCompletedEventHandler DownloadFileCompleted {
            add => baseFactory.DownloadFileCompleted += value;
            remove => baseFactory.DownloadFileCompleted -= value;
        }

        public PooledWebDriverFactory() :
            this(WebDriverOptions.Default) {
        }
        public PooledWebDriverFactory(ILogger logger) :
            this(WebDriverOptions.Default, logger) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions) :
            this(webDriverOptions, PooledWebDriverFactoryOptions.Default) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions, ILogger logger) :
          this(webDriverOptions, PooledWebDriverFactoryOptions.Default, logger) {
        }
        public PooledWebDriverFactory(IPooledWebDriverFactoryOptions webDriverFactoryOptions) :
            this(WebDriverOptions.Default, webDriverFactoryOptions) {
        }
        public PooledWebDriverFactory(IPooledWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
           this(WebDriverOptions.Default, webDriverFactoryOptions, logger) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions, IPooledWebDriverFactoryOptions webDriverFactoryOptions) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions, IPooledWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) :
            this(HttpWebRequestFactory.Default, webDriverOptions, webDriverFactoryOptions, logger) {
        }
        public PooledWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IPooledWebDriverFactoryOptions webDriverFactoryOptions) :
            this(webRequestFactory, webDriverOptions, webDriverFactoryOptions, Logger.Null) {
        }
        public PooledWebDriverFactory(IHttpWebRequestFactory webRequestFactory, IWebDriverOptions webDriverOptions, IPooledWebDriverFactoryOptions webDriverFactoryOptions, ILogger logger) {

            if (webDriverOptions is null)
                throw new ArgumentNullException(nameof(webDriverOptions));

            if (webDriverFactoryOptions is null)
                throw new ArgumentNullException(nameof(webDriverFactoryOptions));

            if (webRequestFactory is null)
                throw new ArgumentNullException(nameof(webRequestFactory));

            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = new NamedLogger(logger, nameof(PooledWebDriverFactory));
            options = webDriverFactoryOptions;
            baseFactory = new WebDriverFactory(webRequestFactory, webDriverOptions, webDriverFactoryOptions, logger);

        }

        public IWebDriver Create() {

            return CreateInternal(webBrowserInfo: options.WebBrowser);

        }
        public IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            return CreateInternal(webBrowserInfo);

        }

        public void Dispose() {

            if (!isDisposed) {

                lock (poolLock) {

                    if (isDisposed)
                        return;

                    ReleaseAllWebDrivers();

                    // Release all threads currently waiting for access to the pool, allowing the wait handle to be safely disposed.

                    while (!poolAccessWaiter.WaitOne(0))
                        poolAccessWaiter.Set();

                    poolAccessWaiter.Dispose();

                    if (disposeFactory)
                        baseFactory.Dispose();

                    isDisposed = true;

                }

            }

        }

        // Private members

        private class WebDriverWrapper :
            IWebDriverWrapper,
            IWebDriver {

            // Public members

            public string Url {
                get => webDriver.Url;
                set => webDriver.Url = value;
            }
            public string Title => webDriver.Title;
            public string PageSource => webDriver.PageSource;
            public string CurrentWindowHandle => webDriver.CurrentWindowHandle;
            public ReadOnlyCollection<string> WindowHandles => webDriver.WindowHandles;

            public WebDriverWrapper(IWebDriver webDriver, PooledWebDriverFactory sourceFactory) {

                this.webDriver = webDriver;
                this.sourceFactory = sourceFactory;

            }

            public void Close() {

                webDriver.Close();

            }
            public void Dispose() {

                if (!isDisposed)
                    sourceFactory.ReleaseWebDriver(webDriver, disposeWebDriver: false);

                isDisposed = true;

            }
            public IWebElement FindElement(By by) {

                return webDriver.FindElement(by);

            }
            public ReadOnlyCollection<IWebElement> FindElements(By by) {

                return webDriver.FindElements(by);

            }
            public IOptions Manage() {

                return webDriver.Manage();

            }
            public INavigation Navigate() {

                return webDriver.Navigate();

            }
            public void Quit() {

                webDriver.Quit();

            }
            public ITargetLocator SwitchTo() {

                return webDriver.SwitchTo();

            }

            public IWebDriver GetWebDriver() {

                return webDriver;

            }

            // Private members

            private readonly IWebDriver webDriver;
            private readonly PooledWebDriverFactory sourceFactory;
            private bool isDisposed = false;

        }

        private class PoolItem {

            public int Id { get; set; }
            public WebBrowserId WebBrowserId { get; set; }
            public IWebDriver WebDriver { get; set; }

            public PoolItem(int id, WebBrowserId webBrowserId, IWebDriver webDriver) {

                this.Id = id;
                this.WebBrowserId = webBrowserId;
                this.WebDriver = webDriver;

            }

        }

        private readonly IPooledWebDriverFactoryOptions options;
        private readonly IWebDriverFactory baseFactory;
        private readonly ILogger logger;
        private readonly bool disposeFactory;
        private readonly List<PoolItem> pool = new List<PoolItem>();
        private readonly List<PoolItem> spawnedDrivers = new List<PoolItem>();
        private readonly object poolLock = new object();
        private readonly AutoResetEvent poolAccessWaiter = new AutoResetEvent(false);
        private int currentWebDriverId = 0;
        private bool isDisposed = false;

        private PooledWebDriverFactory(IWebDriverFactory baseFactory, bool disposeFactory, IPooledWebDriverFactoryOptions options) {

            if (baseFactory is null)
                throw new ArgumentNullException(nameof(baseFactory));

            this.options = options;
            this.baseFactory = baseFactory;
            this.disposeFactory = disposeFactory;

            if (options.PoolSize < 1)
                throw new ArgumentOutOfRangeException(nameof(options), ExceptionMessages.PoolSizeMustBeAtLeast1);

        }

        private IWebDriver CreateInternal(IWebBrowserInfo webBrowserInfo) {

            // Attempt to take a web driver from the pool.

            IWebDriver webDriver = TakeWebDriverFromPool(webBrowserInfo);

            // If no web drivers were available, wait until one is available.

            if (webDriver is null && !isDisposed && poolAccessWaiter is object) {

                logger.Info($"Thread {Thread.CurrentThread.ManagedThreadId} waiting to take web driver from the pool");

                if (poolAccessWaiter.WaitOne(options.Timeout))
                    webDriver = TakeWebDriverFromPool(webBrowserInfo);

            }

            return new WebDriverWrapper(webDriver, this);

        }

        private void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver) {

            lock (poolLock) {

                if (!isDisposed) {

                    PoolItem webDriverItem = spawnedDrivers.Where(item => item.WebDriver == webDriver)
                        .FirstOrDefault();

                    if (webDriverItem is null)
                        throw new ArgumentNullException(nameof(webDriver));

                    if (disposeWebDriver || webDriver.HasQuit()) {

                        // Close and dispose of the web driver entirely.

                        if (!webDriver.HasQuit())
                            webDriver.Quit();

                        webDriver.Dispose();

                        spawnedDrivers.Remove(webDriverItem);

                        logger.Info($"Removed web driver {webDriverItem.Id} from the pool");

                    }
                    else {

                        // Return the web driver to the pool.

                        webDriver.Navigate().GoToUrl("about:blank");

                        pool.Add(webDriverItem);

                        logger.Info($"Returned web driver {webDriverItem.Id} to the pool");

                    }

                    // Allow the next thread waiting for a web driver to continue.

                    poolAccessWaiter.Set();

                }

            }

        }
        private void ReleaseAllWebDrivers() {

            // Close and dispose of any created drivers.

            foreach (IWebDriver webDriver in spawnedDrivers.Select(item => item.WebDriver).ToArray())
                ReleaseWebDriver(webDriver, disposeWebDriver: true);

        }

        private IWebDriver SpawnNewWebDriver(IWebBrowserInfo webBrowserInfo) {

            PoolItem webDriverItem = null;

            lock (poolLock) {

                if (spawnedDrivers.Count() >= options.PoolSize)
                    throw new InvalidOperationException(string.Format(ExceptionMessages.MaximumNumberOfWebDriversReachedWithCount, spawnedDrivers.Count()));

                if (!isDisposed) {

                    IWebDriver webDriver = webBrowserInfo is null ? baseFactory.Create() : baseFactory.Create(webBrowserInfo);

                    webDriverItem = new PoolItem(currentWebDriverId++, webBrowserInfo?.Id ?? WebBrowserId.Unknown, webDriver);

                    spawnedDrivers.Add(webDriverItem);

                    if (webBrowserInfo is null)
                        logger.Info($"Spawned new web driver with ID {webDriverItem.Id}");
                    else
                        logger.Info($"Spawned new web driver with ID {webDriverItem.Id} ({webBrowserInfo})");

                }

            }

            return webDriverItem?.WebDriver;

        }
        private IWebDriver TakeWebDriverFromPool(IWebBrowserInfo webBrowserInfo) {

            lock (poolLock) {

                if (!isDisposed) {

                    if (pool.Count() > 0) {

                        PoolItem webDriverItem = pool.FirstOrDefault(item => (webBrowserInfo?.Id ?? WebBrowserId.Unknown) == item.WebBrowserId);

                        if (webDriverItem is object) {

                            // If there is a driver in the pool that matches the requested web browser, use it.

                            pool.Remove(webDriverItem);

                            logger.Info($"Took web driver {webDriverItem.Id} from the pool");

                            return webDriverItem.WebDriver;

                        }
                        else {

                            // There is no driver in the pool that matches the requested web browser, so remove one and replace it.

                            ReleaseWebDriver(pool.First().WebDriver, disposeWebDriver: true);

                            return SpawnNewWebDriver(webBrowserInfo);

                        }

                    }
                    else if (spawnedDrivers.Count() < options.PoolSize) {

                        // If we haven't spawned the maximum amount of drivers yet, create a new one to use.

                        return SpawnNewWebDriver(webBrowserInfo);

                    }

                }

            }

            // If we get here, we weren't able to get or spawn a web driver (object already disposed?).

            return null;

        }

    }

}
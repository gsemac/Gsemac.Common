using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public class PooledWebDriverFactory :
        WebDriverFactoryBase {

        // Public members

        public PooledWebDriverFactory() :
            this(PooledWebDriverFactoryOptions.Default) {
        }
        public PooledWebDriverFactory(IPooledWebDriverFactoryOptions options) :
            this(new WebDriverFactory(), options) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions, IPooledWebDriverFactoryOptions options) :
            this(webDriverOptions, WebDriverFactoryOptions.Default, options) {
        }
        public PooledWebDriverFactory(IWebDriverOptions webDriverOptions, IWebDriverFactoryOptions webDriverFactoryOptions, IPooledWebDriverFactoryOptions options) :
            this(new WebDriverFactory(webDriverOptions, webDriverFactoryOptions), disposeFactory: true, options) {
        }
        public PooledWebDriverFactory(IWebDriverFactory baseFactory, IPooledWebDriverFactoryOptions options) :
            this(baseFactory, disposeFactory: false, options) {
        }

        public override IWebDriver Create() {

            return GetWebDriverInternal(webBrowserInfo: null);

        }
        public override IWebDriver Create(IWebBrowserInfo webBrowserInfo) {

            return GetWebDriverInternal(webBrowserInfo);

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                lock (poolLock) {

                    isDisposed = true;

                    ReleaseAllWebDrivers();

                    // Release all threads currently waiting for access to the pool, allowing the wait handle to be safely disposed.

                    while (!poolAccessWaiter.WaitOne(0))
                        poolAccessWaiter.Set();

                    poolAccessWaiter.Dispose();

                    if (disposeFactory)
                        baseFactory.Dispose();

                }

            }

        }

        // Private members

        private class WebDriverWrapper :
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
                    sourceFactory.ReleaseWebDriverInternal(webDriver, disposeWebDriver: false);

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

            // Private members

            private readonly IWebDriver webDriver;
            private readonly PooledWebDriverFactory sourceFactory;
            private bool isDisposed = false;

        }

        private class PoolItem {

            public int Id { get; set; }
            public IWebDriver WebDriver { get; set; }

            public PoolItem(int id, IWebDriver webDriver) {

                this.Id = id;
                this.WebDriver = webDriver;

            }

        }

        private readonly IPooledWebDriverFactoryOptions options;
        private readonly IWebDriverFactory baseFactory;
        private readonly bool disposeFactory;
        private readonly Queue<PoolItem> pool = new Queue<PoolItem>();
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
                throw new ArgumentOutOfRangeException(nameof(options), "The pool size must be at least 1.");

            baseFactory.Log += (sender, e) => OnLog.OnLog(e.Message);

        }

        private IWebDriver GetWebDriverInternal(IWebBrowserInfo webBrowserInfo) {

            // Attempt to take a web driver from the pool.

            IWebDriver webDriver = SpawnOrGetWebDriverFromPool(webBrowserInfo);

            // If no web drivers were available, wait until one is available.

            if (webDriver is null && !isDisposed && poolAccessWaiter is object) {

                if (poolAccessWaiter.WaitOne(options.Timeout))
                    webDriver = SpawnOrGetWebDriverFromPool(webBrowserInfo);

            }

            return new WebDriverWrapper(webDriver, this);

        }
        private void ReleaseWebDriverInternal(IWebDriver webDriver, bool disposeWebDriver) {

            lock (poolLock) {

                if (!isDisposed) {

                    PoolItem webDriverItem = spawnedDrivers.Where(item => item.WebDriver == webDriver)
                        .FirstOrDefault();

                    if (webDriverItem is null)
                        throw new ArgumentException(nameof(webDriver), "The given driver is not owned by this pool.");

                    if (disposeWebDriver || webDriver.HasQuit()) {

                        // Close and dispose of the web driver entirely.

                        if (!webDriver.HasQuit())
                            webDriver.Quit();

                        webDriver.Dispose();

                        spawnedDrivers.Remove(webDriverItem);

                        OnLog.Info($"Removed web driver {webDriverItem.Id} from the pool");

                    }
                    else {

                        // Return the web driver to the pool.

                        webDriver.Navigate().GoToUrl("about:blank");

                        pool.Enqueue(webDriverItem);

                        OnLog.Info($"Returned web driver {webDriverItem.Id} to the pool");

                    }

                    // Allow the next thread waiting for a web driver to continue.

                    poolAccessWaiter.Set();

                }

            }

        }
        private void ReleaseAllWebDrivers() {

            // Close and dispose of any created drivers.

            foreach (IWebDriver webDriver in spawnedDrivers.ToArray())
                ReleaseWebDriverInternal(webDriver, disposeWebDriver: true);

        }

        private IWebDriver SpawnOrGetWebDriverFromPool(IWebBrowserInfo webBrowserInfo) {

            PoolItem webDriverItem = null;

            lock (poolLock) {

                if (!isDisposed) {

                    if (pool.Count() > 0) {

                        // If there is a driver in the pool we can use, use it.

                        webDriverItem = pool.Dequeue();

                        OnLog.Info($"Took web driver {webDriverItem.Id} from the pool");

                    }
                    else if (spawnedDrivers.Count() < options.PoolSize) {

                        // If we haven't spawned the maximum amount of drivers yet, create a new one to use.

                        webDriverItem = new PoolItem(currentWebDriverId++, webBrowserInfo is null ? baseFactory.Create() : baseFactory.Create(webBrowserInfo));

                        spawnedDrivers.Add(webDriverItem);

                        if (webBrowserInfo is null)
                            OnLog.Info($"Spawned new web driver with ID {webDriverItem.Id}");
                        else
                            OnLog.Info($"Spawned new web driver with ID {webDriverItem.Id} ({webBrowserInfo})");

                    }

                }

            }

            return webDriverItem?.WebDriver;

        }

    }

}
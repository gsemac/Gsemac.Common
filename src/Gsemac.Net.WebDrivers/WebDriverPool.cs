using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverPool :
        WebDriverPoolBase {

        // Public members

        public WebDriverPool(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions, int poolSize) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (webDriverOptions is null)
                throw new ArgumentNullException(nameof(webDriverOptions));

            this.webBrowserInfo = webBrowserInfo;
            this.webDriverOptions = webDriverOptions;
            this.poolSize = poolSize;

            if (poolSize < 1)
                throw new ArgumentOutOfRangeException(nameof(poolSize), "The pool size must be at least 1.");

        }

        public override IWebDriver GetWebDriver() {

            return GetWebDriverInternal(null);

        }
        public IWebDriver GetWebDriver(TimeSpan timeout) {

            return GetWebDriverInternal(timeout);

        }
        public override void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false) {

            if (webDriver != null && !webDriver.HasQuit())
                webDriver.Navigate().GoToUrl("about:blank");

            ReleaseWebDriverInternal(webDriver, disposeWebDriver);

        }

        public override void Clear() {

            // Close and dispose of any created drivers.

            foreach (IWebDriver webDriver in spawnedDrivers.ToArray())
                ReleaseWebDriver(webDriver, disposeWebDriver: true);

        }

        // Protected members

        protected override void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                lock (poolLock) {

                    isDisposed = true;

                    Clear();

                    // Release all threads currently waiting for access to the pool, allowing the wait handle to be safely disposed.

                    while (!poolAccessWaiter.WaitOne(0))
                        poolAccessWaiter.Set();

                    poolAccessWaiter.Dispose();

                }

            }

        }

        // Private members

        private class PoolItem {

            public int Id { get; set; }
            public IWebDriver WebDriver { get; set; }

            public PoolItem(int id, IWebDriver webDriver) {

                this.Id = id;
                this.WebDriver = webDriver;

            }

        }

        private readonly int poolSize;
        private readonly IWebBrowserInfo webBrowserInfo;
        private readonly IWebDriverOptions webDriverOptions;
        private readonly Queue<PoolItem> pool = new Queue<PoolItem>();
        private readonly List<PoolItem> spawnedDrivers = new List<PoolItem>();
        private readonly object poolLock = new object();
        private readonly AutoResetEvent poolAccessWaiter = new AutoResetEvent(false);
        private int currentWebDriverId = 0;
        private bool isDisposed = false;

        private IWebDriver GetWebDriverInternal(TimeSpan? timeout) {

            // Attempt to take a web driver from the pool.

            IWebDriver webDriver = SpawnOrGetWebDriverFromPool();

            // If no web drivers were available, wait until one is available.

            if (webDriver is null && !isDisposed && poolAccessWaiter != null) {

                if (timeout.HasValue)
                    poolAccessWaiter.WaitOne(timeout.Value);
                else
                    poolAccessWaiter.WaitOne();

                webDriver = SpawnOrGetWebDriverFromPool();

            }

            return webDriver;

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

                        pool.Enqueue(webDriverItem);

                        OnLog.Info($"Returned web driver {webDriverItem.Id} to the pool");

                    }

                    // Allow the next thread waiting for a web driver to continue.

                    poolAccessWaiter.Set();

                }

            }

        }

        private IWebDriver SpawnOrGetWebDriverFromPool() {

            PoolItem webDriverItem = null;

            lock (poolLock) {

                if (!isDisposed) {

                    if (pool.Count() > 0) {

                        // If there is a driver in the pool we can use, use it.

                        webDriverItem = pool.Dequeue();

                        OnLog.Info($"Took web driver {webDriverItem.Id} from the pool");

                    }
                    else if (spawnedDrivers.Count() < poolSize) {

                        // If we haven't spawned the maximum amount of drivers yet, create a new one to use.

                        webDriverItem = new PoolItem(currentWebDriverId++, WebDriver.Create(webBrowserInfo, webDriverOptions));

                        spawnedDrivers.Add(webDriverItem);

                        OnLog.Info($"Spawned new web driver with ID {webDriverItem.Id} ({webBrowserInfo})");

                    }

                }

            }

            return webDriverItem?.WebDriver;

        }

    }

}
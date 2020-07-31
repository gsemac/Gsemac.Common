using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gsemac.Net.SeleniumUtilities {

    public class FixedCapacityWebDriverPool :
        IWebDriverPool {

        // Public members

        public FixedCapacityWebDriverPool(IWebDriverOptions webDriverOptions, int poolSize) {

            this.driverOptions = webDriverOptions;
            this.poolSize = poolSize;

            if (poolSize < 1)
                throw new ArgumentOutOfRangeException(nameof(poolSize), "The pool size must be at least 1.");

        }

        public IWebDriver GetWebDriver() {

            return GetWebDriverInternal(null);

        }
        public IWebDriver GetWebDriver(TimeSpan timeout) {

            return GetWebDriverInternal(timeout);

        }
        public void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false) {

            webDriver.Navigate().GoToUrl("about:blank");

            ReleaseWebDriverInternal(webDriver, disposeWebDriver);

        }

        public void Dispose() {

            Dispose(true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (disposing && !isDisposed) {

                lock (poolLock) {

                    isDisposed = true;

                    // Close and dispose of any created drivers.

                    foreach (IWebDriver driver in spawnedDrivers) {

                        driver.Quit();
                        driver.Dispose();

                    }

                    pool.Clear();
                    spawnedDrivers.Clear();

                    // Release all threads currently waiting for access to the pool, allowing the wait handle to be safely disposed.

                    while (!poolAccessWaiter.WaitOne(0))
                        poolAccessWaiter.Set();

                    poolAccessWaiter.Dispose();

                }

            }

        }

        // Private members

        private readonly int poolSize;
        private readonly IWebDriverOptions driverOptions;
        private readonly Queue<IWebDriver> pool = new Queue<IWebDriver>();
        private readonly List<IWebDriver> spawnedDrivers = new List<IWebDriver>();
        private readonly object poolLock = new object();
        private readonly AutoResetEvent poolAccessWaiter = new AutoResetEvent(false);
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

                    if (!spawnedDrivers.Any(driver => driver == webDriver))
                        throw new ArgumentException(nameof(webDriver), "The given driver is not owned by this pool.");

                    if (disposeWebDriver) {

                        // Close and dispose of the web driver entirely.

                        webDriver.Quit();

                        webDriver.Dispose();

                        spawnedDrivers.Remove(webDriver);

                    }
                    else {

                        // Return the web driver to the pool.

                        pool.Enqueue(webDriver);

                    }

                    // Allow the next thread waiting for a web driver to continue.

                    poolAccessWaiter.Set();

                }

            }

        }

        private IWebDriver SpawnOrGetWebDriverFromPool() {

            IWebDriver webDriver = null;

            lock (poolLock) {

                if (!isDisposed) {

                    if (pool.Count() > 0) {

                        // If there is a driver in the pool we can use, use it.

                        webDriver = pool.Dequeue();

                    }
                    else if (spawnedDrivers.Count() < poolSize) {

                        // If we haven't spawned the maximum amount of drivers yet, create a new one to use.

                        webDriver = WebDriverUtilities.CreateWebDriver(driverOptions);

                        spawnedDrivers.Add(webDriver);

                    }

                }

            }

            return webDriver;

        }

    }

}
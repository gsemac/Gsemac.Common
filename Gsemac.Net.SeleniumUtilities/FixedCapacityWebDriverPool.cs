using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Gsemac.Net.SeleniumUtilities {

    public class FixedCapacityWebDriverPool :
        IWebDriverPool {

        // Public members

        public FixedCapacityWebDriverPool(Func<IWebDriver> createWebDriverFunc, int poolSize) {

            this.createWebDriverFunc = createWebDriverFunc;
            this.poolSize = poolSize;
            poolSemaphore = new SemaphoreSlim(poolSize);

        }

        public IWebDriver GetWebDriver() {

            return GetWebDriverInternal(null);

        }
        public IWebDriver GetWebDriver(TimeSpan timeout) {

            return GetWebDriverInternal(timeout);

        }
        public void ReleaseWebDriver(IWebDriver webDriver) {

            // If there are threads waiting for a web driver, we'll put it in the queue.
            // Otherwise, we'll dispose of the web driver to avoid hanging onto it unnecessarily.

            if (numberOfWaitingThreads > 0 && pool.Count < poolSize) {

                pool.Enqueue(webDriver);

            }
            else {

                webDriver.Dispose();

            }

            poolSemaphore.Release();

        }

        // Private members

        private readonly int poolSize;
        private readonly Func<IWebDriver> createWebDriverFunc;
        private readonly ConcurrentQueue<IWebDriver> pool = new ConcurrentQueue<IWebDriver>();
        private readonly SemaphoreSlim poolSemaphore;
        private int numberOfWaitingThreads = 0;

        private IWebDriver GetWebDriverInternal(TimeSpan? timeout) {

            Interlocked.Increment(ref numberOfWaitingThreads);

            IWebDriver webDriver = null;
            bool enteredSemaphore = true;

            if (timeout.HasValue)
                enteredSemaphore = poolSemaphore.Wait((int)timeout.Value.TotalMilliseconds);
            else
                poolSemaphore.Wait();

            if (enteredSemaphore && !pool.TryDequeue(out webDriver)) {

                // There are no web drivers available in the pool, so create a new one.

                webDriver = createWebDriverFunc();

            }

            Interlocked.Decrement(ref numberOfWaitingThreads);

            return webDriver;

        }

    }

}
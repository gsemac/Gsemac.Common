using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace Gsemac.Net.SeleniumUtilities {

    public class AutoReleasingWebDriver :
        IWebDriver {

        // Public members

        public string Url {
            get => underlyingWebDriver.Url;
            set => underlyingWebDriver.Url = value;
        }
        public string Title => underlyingWebDriver.Title;
        public string PageSource => underlyingWebDriver.PageSource;
        public string CurrentWindowHandle => underlyingWebDriver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => underlyingWebDriver.WindowHandles;

        public AutoReleasingWebDriver(IWebDriver underlyingWebDriver, IWebDriverPool pool) {

            if (underlyingWebDriver is null)
                throw new ArgumentNullException(nameof(underlyingWebDriver));

            if (pool is null)
                throw new ArgumentNullException(nameof(pool));

            if (underlyingWebDriver is AutoReleasingWebDriver)
                throw new ArgumentException("Another instance of this class cannot be wrapped.");

            this.underlyingWebDriver = underlyingWebDriver;
            this.pool = pool;

        }

        public void Close() => underlyingWebDriver.Close();
        public IWebElement FindElement(By by) => underlyingWebDriver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => underlyingWebDriver.FindElements(by);
        public IOptions Manage() => underlyingWebDriver.Manage();
        public INavigation Navigate() => underlyingWebDriver.Navigate();
        public void Quit() => underlyingWebDriver.Quit();
        public ITargetLocator SwitchTo() => underlyingWebDriver.SwitchTo();

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected virtual void Dispose(bool disposing) {

            if (!disposedValue) {

                if (disposing) {

                    if (underlyingWebDriver != null && pool != null)
                        pool.ReleaseWebDriver(underlyingWebDriver);

                }

                disposedValue = true;

            }
        }

        // Private members

        private readonly IWebDriver underlyingWebDriver;
        private readonly IWebDriverPool pool;
        private bool disposedValue;

    }

}
using Gsemac.IO.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverPoolBase :
        IWebDriverPool {

        // Public members

        public event LogEventHandler Log;

        public abstract IWebDriver GetWebDriver();
        public abstract void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false);
        public abstract void Clear();

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected LogEventHelper OnLog => new LogEventHelper("Web Driver Pool", Log);

        protected virtual void Dispose(bool disposing) { }

    }

}
using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverFactoryBase :
        IWebDriverFactory {

        // Public members

        public event LogEventHandler Log;

        public abstract IWebDriver Create();
        public abstract IWebDriver Create(IWebBrowserInfo webBrowserInfo);

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected LogEventHelper OnLog => new LogEventHelper("Web Driver Factory", Log);

        protected virtual void Dispose(bool disposing) { }

    }

}
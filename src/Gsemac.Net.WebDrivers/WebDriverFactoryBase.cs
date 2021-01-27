using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverFactoryBase :
        IWebDriverFactory {

        // Public members

        public event LogEventHandler Log;

        public void Dispose() {

            Dispose(disposing: true);

            GC.SuppressFinalize(this);

        }

        // Protected members

        protected LogEventHelper OnLog => new LogEventHelper("Web Driver Factory", Log);

        protected virtual void Dispose(bool disposing) { }

        // Private members

        public abstract IWebDriver Create(IWebBrowserInfo webBrowserInfo);

    }

}
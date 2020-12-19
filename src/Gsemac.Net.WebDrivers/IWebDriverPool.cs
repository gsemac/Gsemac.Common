using Gsemac.IO.Logging;
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverPool :
        ILoggable,
        IDisposable {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false);

        void Clear();

    }

}
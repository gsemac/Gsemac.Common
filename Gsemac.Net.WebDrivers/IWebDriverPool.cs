using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverPool :
        IDisposable {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false);

    }

}
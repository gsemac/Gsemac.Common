using OpenQA.Selenium;
using System;

namespace Gsemac.Net.WebDriverUtilities {

    public interface IWebDriverPool :
        IDisposable {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false);

    }

}
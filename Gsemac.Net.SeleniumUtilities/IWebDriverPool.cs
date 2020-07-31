using OpenQA.Selenium;
using System;

namespace Gsemac.Net.SeleniumUtilities {

    public interface IWebDriverPool :
        IDisposable {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver, bool disposeWebDriver = false);

    }

}
using OpenQA.Selenium;
using System;

namespace Gsemac.Net.SeleniumUtilities {

    public interface IWebDriverPool {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver);

    }

}
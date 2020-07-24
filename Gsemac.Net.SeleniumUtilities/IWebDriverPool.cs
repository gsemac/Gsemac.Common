using OpenQA.Selenium;
using System;

namespace Gsemac.Net.Selenium {

    public interface IWebDriverPool {

        IWebDriver GetWebDriver();
        void ReleaseWebDriver(IWebDriver webDriver);

    }

}
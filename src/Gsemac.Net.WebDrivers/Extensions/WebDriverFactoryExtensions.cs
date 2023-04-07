using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class WebDriverFactoryExtensions {

        public static IWebDriver Create(this IWebDriverFactory webDriverFactory, BrowserId webBrowserId) {

            return webDriverFactory.Create(BrowserInfoFactory.Default.GetBrowserInfo(webBrowserId));

        }

    }

}
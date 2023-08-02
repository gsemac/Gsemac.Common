using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class WebDriverFactoryExtensions {

        public static IWebDriver Create(this IWebDriverFactory webDriverFactory, WebBrowserId webBrowserId) {

            return webDriverFactory.Create(WebBrowserInfoFactory.Default.GetWebBrowserInfo(webBrowserId));

        }

    }

}
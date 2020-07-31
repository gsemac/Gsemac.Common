using OpenQA.Selenium;

namespace Gsemac.Net.SeleniumUtilities.Extensions {

    public static class WebDriverExtensions {

        public static bool HasQuit(this IWebDriver webDriver) {

            return string.IsNullOrEmpty(webDriver.ToString());

        }

    }

}
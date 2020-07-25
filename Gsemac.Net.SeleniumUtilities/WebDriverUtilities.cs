using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Net;

namespace Gsemac.Net.SeleniumUtilities {

    public static class WebDriverUtilities {

        // Public members

        public static IWebDriver CreateFirefoxWebDriver(IWebDriverOptions options, Uri uri) {

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = options.BrowserExecutablePath
            };

            FirefoxDriverService driverService = string.IsNullOrEmpty(options.WebDriverExecutablePath) ?
                FirefoxDriverService.CreateDefaultService() :
                FirefoxDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(options.WebDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArguments("-width=1024");
            driverOptions.AddArguments("-height=768");

            if (options.Headless)
                driverOptions.AddArgument("--headless");

            FirefoxProfile profile = new FirefoxProfile {
                DeleteAfterUse = true
            };

            if (!string.IsNullOrEmpty(options.UserAgent))
                profile.SetPreference("general.useragent.override", options.UserAgent);

            if (options.Proxy != null) {

                string proxyAbsoluteUri = options.Proxy.GetProxy(uri).AbsoluteUri;

                Proxy proxy = new Proxy {
                    HttpProxy = proxyAbsoluteUri,
                    SslProxy = proxyAbsoluteUri
                };

                driverOptions.Proxy = proxy;

            }

            // This preference disables the "navigator.webdriver" property.

            profile.SetPreference("dom.webdriver.enabled", false);

            driverOptions.Profile = profile;

            IWebDriver driver = new FirefoxDriver(driverService, driverOptions);

            return driver;

        }
        public static IWebDriver CreateChromeWebDriver(IWebDriverOptions options, Uri uri) {

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = options.BrowserExecutablePath
            };

            ChromeDriverService driverService = string.IsNullOrEmpty(options.WebDriverExecutablePath) ?
                ChromeDriverService.CreateDefaultService() :
                ChromeDriverService.CreateDefaultService(System.IO.Path.GetDirectoryName(options.WebDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArgument("--window-size=1024,768");

            if (options.Headless)
                driverOptions.AddArgument("--headless");

            if (!string.IsNullOrEmpty(options.UserAgent))
                driverOptions.AddArgument($"--user-agent={options.UserAgent}");

            if (options.Proxy != null)
                driverOptions.AddArgument($"--proxy-server={options.Proxy.GetProxy(uri).AbsoluteUri}");

            IWebDriver driver = new ChromeDriver(driverService, driverOptions);

            return driver;

        }
        public static IWebDriver CreateWebDriver(IWebDriverOptions options, Uri uri = null) {

            uri = uri ?? new Uri("http://example.com/");

            string browserExecutableFileName = System.IO.Path.GetFileNameWithoutExtension(options.BrowserExecutablePath);

            if (browserExecutableFileName.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return CreateFirefoxWebDriver(options, uri);
            else if (browserExecutableFileName.Equals("chrome", StringComparison.OrdinalIgnoreCase))
                return CreateChromeWebDriver(options, uri);
            else
                throw new ArgumentException("The given browser executable was not recognized.");

        }

        public static string GetUserAgent(IWebDriver driver) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;

            string userAgent = (string)javascriptExecutor.ExecuteScript("return navigator.userAgent");

            return userAgent;

        }
        public static CookieCollection GetCookies(IWebDriver driver) {

            CookieCollection cookies = new CookieCollection();

            foreach (OpenQA.Selenium.Cookie cookie in driver.Manage().Cookies.AllCookies) {

                System.Net.Cookie netCookie = new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain) {
                    HttpOnly = cookie.IsHttpOnly,
                    Secure = cookie.Secure
                };

                if (cookie.Expiry.HasValue)
                    netCookie.Expires = cookie.Expiry.Value;

                cookies.Add(netCookie);

            }

            return cookies;

        }
        public static void AddCookies(IWebDriver driver, CookieCollection cookies) {

            foreach (System.Net.Cookie cookie in cookies) {

                driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expires));

            }

        }

        public static void HideOtherElements(IWebDriver driver, string elementXPath) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;

            javascriptExecutor.ExecuteScript(Properties.Resources.HideOtherElementsJs);
            javascriptExecutor.ExecuteScript($"window[\"hiddenElements\"] = hideOtherElements({elementXPath});");

        }
        public static void RestoreHiddenElements(IWebDriver driver) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;

            javascriptExecutor.ExecuteScript(Properties.Resources.HideOtherElementsJs);
            javascriptExecutor.ExecuteScript($"return restoreHiddenElements(window[\"hiddenElements\"]);");


        }

    }

}
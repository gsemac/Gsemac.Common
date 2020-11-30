using Gsemac.Net.WebBrowsers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public static class WebDriverUtilities {

        // Public members

        public static IWebDriver CreateFirefoxWebDriver(IWebDriverOptions options, Uri uri = null) {

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(options.WebDriverExecutablePath);

            // Create the driver service.

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = options.BrowserExecutablePath
            };

            FirefoxDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                FirefoxDriverService.CreateDefaultService() :
                FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArguments($"--width={options.WindowSize.Width}");
            driverOptions.AddArguments($"--height={options.WindowSize.Height}");

            // Apply headless setting.

            if (options.Headless)
                driverOptions.AddArgument("--headless");

            // Apply user agent.

            FirefoxProfile profile = new FirefoxProfile {
                DeleteAfterUse = true
            };

            if (!string.IsNullOrEmpty(options.UserAgent))
                profile.SetPreference("general.useragent.override", options.UserAgent);

            // If the user specified a proxy, apply the proxy.

            if (!(options.Proxy is null)) {

                string proxyAbsoluteUri = options.Proxy.GetProxy(uri ?? new Uri("http://example.com")).AbsoluteUri;

                Proxy proxy = new Proxy {
                    HttpProxy = proxyAbsoluteUri,
                    SslProxy = proxyAbsoluteUri
                };

                driverOptions.Proxy = proxy;

            }

            // Disable pop-ups.

            if (options.DisablePopUps)
                profile.SetPreference("dom.popup_allowed_events", "");

            // Disable the "navigator.webdriver" property.

            profile.SetPreference("dom.webdriver.enabled", false);

            driverOptions.Profile = profile;

            driverOptions.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)options.PageLoadStrategy;

            IWebDriver driver = new FirefoxDriver(driverService, driverOptions);

            return driver;

        }
        public static IWebDriver CreateChromeWebDriver(IWebDriverOptions options, Uri uri = null) {

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(options.WebDriverExecutablePath);

            // Create the driver service.

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = options.BrowserExecutablePath
            };

            ChromeDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                ChromeDriverService.CreateDefaultService() :
                ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArgument($"--window-size={options.WindowSize.Width},{options.WindowSize.Height}");
            driverOptions.AddArgument($"--window-position={options.WindowPosition.X},{options.WindowPosition.Y}");

            // Apply headless setting.

            if (options.Headless)
                driverOptions.AddArgument("--headless");

            // Apply user agent.

            if (!string.IsNullOrEmpty(options.UserAgent))
                driverOptions.AddArgument($"--user-agent={options.UserAgent}");

            // If the user specified a proxy, apply the proxy.

            if (!(options.Proxy is null))
                driverOptions.AddArgument($"--proxy-server={options.Proxy.GetProxy(uri ?? new Uri("http://example.com")).AbsoluteUri}");

            // Disable the "navigator.webdriver" property.

            driverOptions.AddArgument("--disable-blink-features=AutomationControlled");

            driverOptions.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)options.PageLoadStrategy;

            IWebDriver driver = new ChromeDriver(driverService, driverOptions);

            return driver;

        }
        public static IWebDriver CreateWebDriver(IWebDriverOptions options, Uri uri = null) {

            uri = uri ?? new Uri("http://example.com/");

            IWebBrowserInfo browserInfo = File.Exists(options.BrowserExecutablePath) ?
                new WebBrowserInfo(options.BrowserExecutablePath) :
                null;

            IWebDriver result;

            switch (browserInfo?.Id ?? WebBrowserId.Unidentified) {

                case WebBrowserId.GoogleChrome:

                    result = CreateChromeWebDriver(options, uri);

                    break;

                case WebBrowserId.Firefox:

                    result = CreateFirefoxWebDriver(options, uri);

                    break;

                default:

                    throw new ArgumentException("The given browser was not recognized.");

            }

            result.Manage().Window.Position = options.WindowPosition;

            return result;

        }

        // Private members

        private static string GetFullWebDriverExecutablePath(string webDriverExecutablePath) {

            if (!string.IsNullOrEmpty(webDriverExecutablePath) && !Path.IsPathRooted(webDriverExecutablePath))
                webDriverExecutablePath = Path.GetFullPath(webDriverExecutablePath);

            return webDriverExecutablePath;

        }

    }

}
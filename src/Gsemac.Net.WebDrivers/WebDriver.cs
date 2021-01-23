using Gsemac.Net.Extensions;
using Gsemac.Net.WebBrowsers;
using Gsemac.Net.WebDrivers.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;

namespace Gsemac.Net.WebDrivers {

    public static class WebDriver {

        // Public members

        public static IWebDriver Create(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            if (webBrowserInfo is null)
                throw new ArgumentNullException(nameof(webBrowserInfo));

            if (webDriverOptions is null)
                throw new ArgumentNullException(nameof(webDriverOptions));

            IWebDriver result;

            switch (webBrowserInfo.Id) {

                case WebBrowserId.Chrome:
                    result = CreateChromeWebDriver(webBrowserInfo, webDriverOptions);
                    break;

                case WebBrowserId.Firefox:
                    result = CreateFirefoxWebDriver(webBrowserInfo, webDriverOptions);
                    break;

                default:
                    throw new ArgumentException("The given web browser is not supported.", nameof(webBrowserInfo));

            }

            result.Manage().Window.Position = webDriverOptions.WindowPosition;

            return result;

        }

        // Private members

        private static string GetFullWebDriverExecutablePath(string webDriverExecutablePath) {

            if (!string.IsNullOrWhiteSpace(webDriverExecutablePath) && !Path.IsPathRooted(webDriverExecutablePath))
                webDriverExecutablePath = Path.GetFullPath(webDriverExecutablePath);

            return webDriverExecutablePath;

        }

        private static IWebDriver CreateChromeWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(webDriverOptions.WebDriverExecutablePath);

            // Create the driver service.

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = webBrowserInfo.ExecutablePath
            };

            ChromeDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                ChromeDriverService.CreateDefaultService() :
                ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            if (webDriverOptions.Headless)
                driverOptions.AddArgument("--headless");

            if (!string.IsNullOrEmpty(webDriverOptions.UserAgent))
                driverOptions.AddArgument($"--user-agent={webDriverOptions.UserAgent}");

            if (!webDriverOptions.Proxy.IsEmpty())
                driverOptions.AddArgument($"--proxy-server={webDriverOptions.Proxy.ToProxyString()}");

            driverOptions.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)webDriverOptions.PageLoadStrategy;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArgument($"--window-size={webDriverOptions.WindowSize.Width},{webDriverOptions.WindowSize.Height}");
            driverOptions.AddArgument($"--window-position={webDriverOptions.WindowPosition.X},{webDriverOptions.WindowPosition.Y}");

            // Disable the "navigator.webdriver" property.

            driverOptions.AddArgument("--disable-blink-features=AutomationControlled");

            ChromeDriver driver = new ChromeDriver(driverService, driverOptions);

            if (webDriverOptions.Stealth) {

                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.utils);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.chrome_app);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.chrome_runtime);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.iframe_contentWindow);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.media_codecs);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_languages);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_permissions);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_plugins);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_vendor);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_webdriver);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.webgl_vendor);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.window_outerdimensions);
                driver.AddScriptToEvaluateOnNewDocument(Properties.Resources.navigator_hardwareConcurrency);
                //driver.AddScriptToEvaluateOnNewDocument("(() => { utils = undefined; })();");

            }

            return driver;

        }
        private static IWebDriver CreateFirefoxWebDriver(IWebBrowserInfo webBrowserInfo, IWebDriverOptions webDriverOptions) {

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(webDriverOptions.WebDriverExecutablePath);

            // Create the driver service.

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = webBrowserInfo.ExecutablePath
            };

            FirefoxDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                FirefoxDriverService.CreateDefaultService() :
                FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

            driverService.HideCommandPromptWindow = true;

            if (webDriverOptions.Headless)
                driverOptions.AddArgument("--headless");

            // Apply user agent.

            FirefoxProfile profile = new FirefoxProfile {
                DeleteAfterUse = true
            };

            if (!string.IsNullOrEmpty(webDriverOptions.UserAgent))
                profile.SetPreference("general.useragent.override", webDriverOptions.UserAgent);

            // If the user specified a proxy, apply the proxy.

            if (!webDriverOptions.Proxy.IsEmpty()) {

                string proxyAbsoluteUri = webDriverOptions.Proxy.ToProxyString();

                Proxy proxy = new Proxy {
                    HttpProxy = proxyAbsoluteUri,
                    SslProxy = proxyAbsoluteUri
                };

                driverOptions.Proxy = proxy;

            }

            if (webDriverOptions.DisablePopUps)
                profile.SetPreference("dom.popup_allowed_events", "");

            driverOptions.Profile = profile;

            driverOptions.PageLoadStrategy = (OpenQA.Selenium.PageLoadStrategy)webDriverOptions.PageLoadStrategy;

            // Resize the window to a reasonable resolution so that viewport matches a conventional monitor viewport.

            driverOptions.AddArguments($"--width={webDriverOptions.WindowSize.Width}");
            driverOptions.AddArguments($"--height={webDriverOptions.WindowSize.Height}");

            // Disable the "navigator.webdriver" property.

            profile.SetPreference("dom.webdriver.enabled", false);

            IWebDriver driver = new FirefoxDriver(driverService, driverOptions);

            return driver;

        }

    }

}
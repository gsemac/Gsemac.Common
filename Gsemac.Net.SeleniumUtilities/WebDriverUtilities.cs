using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.SeleniumUtilities {

    public static class WebDriverUtilities {

        // Public members

        public static IWebDriver CreateFirefoxWebDriver(IWebDriverOptions options, Uri uri) {

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(options.WebDriverExecutablePath);

            FirefoxOptions driverOptions = new FirefoxOptions {
                BrowserExecutableLocation = options.BrowserExecutablePath
            };

            FirefoxDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                FirefoxDriverService.CreateDefaultService() :
                FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

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

            string webDriverExecutablePath = GetFullWebDriverExecutablePath(options.WebDriverExecutablePath);

            ChromeOptions driverOptions = new ChromeOptions {
                BinaryLocation = options.BrowserExecutablePath
            };

            ChromeDriverService driverService = string.IsNullOrEmpty(webDriverExecutablePath) ?
                ChromeDriverService.CreateDefaultService() :
                ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(webDriverExecutablePath));

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

        public static Size GetDocumentSize(IWebDriver driver) {

            int totalWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.offsetWidth");
            int totalHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.parentNode.scrollHeight");

            return new Size(totalWidth, totalHeight);

        }
        public static Size GetViewportSize(IWebDriver driver) {

            int viewportWidth = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.clientWidth");
            int viewportHeight = (int)(long)((IJavaScriptExecutor)driver).ExecuteScript("return window.innerHeight");

            return new Size(viewportWidth, viewportHeight);

        }
        public static void ScrollTo(IWebDriver driver, int x, int y) {

            ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo({x}, {y});");

        }
        public static void ScrollBy(IWebDriver driver, int deltaX, int deltaY) {

            ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollBy({deltaX}, {deltaY});");

        }

        public static void HideOtherElements(IWebDriver driver, string elementXPath) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;

            javascriptExecutor.ExecuteScript(Properties.Resources.HideOtherElementsJs + $"\nwindow[\"hiddenElements\"] = hideOtherElements('{elementXPath}');");

        }
        public static void RestoreHiddenElements(IWebDriver driver) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;

            javascriptExecutor.ExecuteScript(Properties.Resources.HideOtherElementsJs + $"\nreturn restoreHiddenElements(window[\"hiddenElements\"]);");

        }

        public static void NonBlockingGoToUrl(IWebDriver driver, string url) {

            // By default, GoToUrl blocks until the page has loaded completely.
            // To avoid blocking, a low timeout is set so that a timeout exception is raised, which is then caught.

            TimeSpan originalTimeout = driver.Manage().Timeouts().PageLoad;

#pragma warning disable CA1031 // Do not catch general exception types
            try {

                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1);
                driver.Navigate().GoToUrl(url);

            }
            catch (WebDriverException) { }
            finally {

                driver.Manage().Timeouts().PageLoad = originalTimeout;

            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

#if NETFRAMEWORK

        public static Bitmap ScreenshotPage(IWebDriver driver) {

            // This implementation was adapted from https://stackoverflow.com/a/31396164/5383169 (Lachlan Goodhew-Cook)

            // Maximize the driver window to maximize the amount of visible content.

            Size originalSize = driver.Manage().Window.Size;

            driver.Manage().Window.Maximize();

            // By default, the screenshot will only contain the window's visible content.
            // In order to get a full page screenshot, we need to take multiple screenshots and stitch them together.

            Size documentSize = GetDocumentSize(driver);
            Size viewportSize = GetViewportSize(driver);

            ScrollTo(driver, 0, 0);

            Bitmap result;

            if (documentSize.Width <= viewportSize.Width && documentSize.Height < viewportSize.Height) {

                // The entire document fits the screen, so there is no need to take multiple screenshots.

                result = ScreenshotToBitmap((driver as ITakesScreenshot).GetScreenshot());

            }
            else {

                // Divide the document area into rectangles, with each one representing the area of an individual screenshot.

                List<Rectangle> rectangles = new List<Rectangle>();

                for (int y = 0; y < documentSize.Height; y += viewportSize.Height) {

                    int height = viewportSize.Height;

                    if (y + viewportSize.Height > documentSize.Height)
                        height = documentSize.Height - y;

                    for (var x = 0; x < documentSize.Width; x += viewportSize.Width) {

                        int width = viewportSize.Width;

                        if (x + viewportSize.Width > documentSize.Width)
                            width = documentSize.Width - x;

                        rectangles.Add(new Rectangle(x, y, width, height));
                    }

                }

                // Build the image.

                result = new Bitmap(documentSize.Width, documentSize.Height);

                Rectangle previousRectangle = Rectangle.Empty;

                foreach (Rectangle rectangle in rectangles) {

                    if (previousRectangle != Rectangle.Empty) {

                        var deltaX = rectangle.Right - previousRectangle.Right;
                        var deltaY = rectangle.Bottom - previousRectangle.Bottom;

                        ScrollBy(driver, deltaX, deltaY);

                    }

                    Rectangle sourceRectangle = new Rectangle(viewportSize.Width - rectangle.Width, viewportSize.Height - rectangle.Height, rectangle.Width, rectangle.Height);

                    using (Bitmap screenshot = ScreenshotToBitmap(TakeScreenshot(driver)))
                    using (Graphics graphics = Graphics.FromImage(result))
                        graphics.DrawImage(screenshot, rectangle, sourceRectangle, GraphicsUnit.Pixel);

                    previousRectangle = rectangle;

                }

            }

            // Restore the original window size.

            driver.Manage().Window.Size = originalSize;

            // Return the result.

            return result;

        }
        public static Bitmap ScreenshotElement(IWebDriver driver, string elementXPath) {

            // Maximize the driver window to maximize the amount of visible content.
            // Even though this is done by ScreenshotPage, it's done here as well so that the element bounds are consistent.

            Size originalSize = driver.Manage().Window.Size;

            driver.Manage().Window.Maximize();

            // Hide all elements except for the one we're interested in.

            HideOtherElements(driver, elementXPath);

            IWebElement element = driver.FindElements(By.XPath(elementXPath)).FirstOrDefault();
            Rectangle elementRect = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);

            Bitmap result;

            using (Bitmap fullPageScreenshot = ScreenshotPage(driver))
                result = fullPageScreenshot.Clone(elementRect, fullPageScreenshot.PixelFormat);

            // Restore the original window state.

            RestoreHiddenElements(driver);

            driver.Manage().Window.Size = originalSize;

            // Return the result.

            return result;

        }

#endif

        // Private members

        private static string GetFullWebDriverExecutablePath(string webDriverExecutablePath) {

            if (!string.IsNullOrEmpty(webDriverExecutablePath) && !Path.IsPathRooted(webDriverExecutablePath))
                webDriverExecutablePath = Path.GetFullPath(webDriverExecutablePath);

            return webDriverExecutablePath;

        }

        private static Screenshot TakeScreenshot(IWebDriver driver) {

            return (driver as ITakesScreenshot).GetScreenshot();

        }

#if NETFRAMEWORK

        private static Bitmap ScreenshotToBitmap(Screenshot screenshot) {

            using (Stream stream = new MemoryStream(screenshot.AsByteArray))
                return new Bitmap(stream);

        }

#endif

    }

}
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

namespace Gsemac.Net.WebDrivers.Extensions {

    public static class WebDriverExtensions {

        internal static IWebDriver UnwrapWebDriver(this IWebDriver driver) {

            if (driver is IWebDriverWrapper wrapper)
                return wrapper.GetWebDriver();

            return driver;

        }

        public static void ExecuteScript(this IWebDriver driver, string script) {

            driver.ExecuteScript<object>(script);

        }
        public static T ExecuteScript<T>(this IWebDriver driver, string script) {

            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)UnwrapWebDriver(driver);

            T result = (T)javascriptExecutor.ExecuteScript(script);

            return result;

        }

        public static string GetUserAgent(this IWebDriver driver) {

            return driver.ExecuteScript<string>("return navigator.userAgent");

        }
        public static bool IsDocumentComplete(this IWebDriver driver) {

            return driver.ExecuteScript<string>("return document.readyState").Equals("complete");

        }
        public static Size GetDocumentSize(this IWebDriver driver) {

            int totalWidth = (int)driver.ExecuteScript<long>("return document.body.offsetWidth");
            int totalHeight = (int)driver.ExecuteScript<long>("return document.body.parentNode.scrollHeight");

            return new Size(totalWidth, totalHeight);

        }
        public static Size GetViewportSize(this IWebDriver driver) {

            int viewportWidth = (int)driver.ExecuteScript<long>("return document.body.clientWidth");
            int viewportHeight = (int)driver.ExecuteScript<long>("return window.innerHeight");

            return new Size(viewportWidth, viewportHeight);

        }
        public static void ScrollTo(this IWebDriver driver, int x, int y) {

            driver.ExecuteScript($"window.scrollTo({x}, {y});");

        }
        public static void ScrollBy(this IWebDriver driver, int deltaX, int deltaY) {

            driver.ExecuteScript($"window.scrollBy({deltaX}, {deltaY});");

        }
        public static void HideOtherElements(this IWebDriver driver, string elementXPath) {

            // The xpath string may contain quotes, so we may need to use different outer quotes depending on what's inside.
            // This isn't foolproof, but should work in the majority of situations.

            string elementXPathWithQuotes = elementXPath.Contains("\"") ?
                $"'{elementXPath}'" :
                $"\"{elementXPath}'\"";

            driver.ExecuteScript(Properties.Resources.HideOtherElementsJs + $"\nwindow[\"hiddenElements\"] = hideOtherElements({elementXPathWithQuotes});");

        }
        public static void RestoreHiddenElements(this IWebDriver driver) {

            driver.ExecuteScript(Properties.Resources.HideOtherElementsJs + $"\nreturn restoreHiddenElements(window[\"hiddenElements\"]);");

        }

        public static CookieCollection GetCookies(this IWebDriver driver) {

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
        public static void AddCookies(this IWebDriver driver, CookieCollection cookies) {

            foreach (System.Net.Cookie netCookie in cookies) {

                DateTime? expiry = netCookie.Expires.Equals(DateTime.MinValue) ? null : (DateTime?)netCookie.Expires;

                OpenQA.Selenium.Cookie cookie = new OpenQA.Selenium.Cookie(netCookie.Name, netCookie.Value, netCookie.Domain, netCookie.Path, expiry);

                // If a cookie with the same name arleady exists, delete it so that it can be replaced.

                OpenQA.Selenium.Cookie existingCookie = driver.Manage().Cookies.GetCookieNamed(cookie.Name);

                if (existingCookie != null)
                    driver.Manage().Cookies.DeleteCookie(existingCookie);

                driver.Manage().Cookies.AddCookie(cookie);

            }

        }

        public static void GoToUrl(this IWebDriver driver, string url, bool blocking = true) {

            if (blocking) {

                driver.Navigate().GoToUrl(url);

                WebDriverWait wait = new WebDriverWait(driver, driver.Manage().Timeouts().PageLoad);

                wait.Until(d => IsDocumentComplete(d));

            }
            else {

                // By default, GoToUrl blocks until the page has loaded completely.
                // To avoid blocking, a low timeout is set so that a timeout exception is raised, which is then caught.

                TimeSpan originalTimeout = driver.Manage().Timeouts().PageLoad;
                try {

                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(1);
                    driver.Navigate().GoToUrl(url);

                }
                catch (WebDriverException) { }
                finally {

                    driver.Manage().Timeouts().PageLoad = originalTimeout;

                }
            }

        }
        public static void GoToBlank(this IWebDriver driver) {

            driver.Navigate().GoToUrl("about:blank");

        }

        public static bool HasQuit(this IWebDriver webDriver) {

            try {

                return !(webDriver?.WindowHandles?.Any()) ?? true;

            }
            catch (WebDriverException) {

                return true;

            }

        }

#if NETFRAMEWORK

        public static Bitmap ScreenshotPage(this IWebDriver driver) {

            // This implementation was adapted from https://stackoverflow.com/a/31396164/5383169 (Lachlan Goodhew-Cook)

            // By default, the screenshot will only contain the window's visible content.
            // In order to get a full page screenshot, we need to take multiple screenshots and stitch them together.

            Size documentSize = GetDocumentSize(driver);
            Size viewportSize = GetViewportSize(driver);

            ScrollTo(driver, 0, 0);

            Bitmap result;

            if (documentSize.Width <= viewportSize.Width && documentSize.Height < viewportSize.Height) {

                // The entire document fits the screen, so there is no need to take multiple screenshots.

                result = ScreenshotToBitmap(driver.TakeScreenshot());

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

            // Return the result.

            return result;

        }
        public static Bitmap ScreenshotElement(this IWebDriver driver, string elementXPath) {

            // Hide all elements except for the one we're interested in.

            HideOtherElements(driver, elementXPath);

            IWebElement element = driver.FindElements(By.XPath(elementXPath)).FirstOrDefault();
            Rectangle elementRect = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);

            Bitmap result;

            using (Bitmap fullPageScreenshot = ScreenshotPage(driver))
                result = fullPageScreenshot.Clone(elementRect, fullPageScreenshot.PixelFormat);

            // Restore the original window state.

            RestoreHiddenElements(driver);

            // Return the result.

            return result;

        }

        // Private members



#endif

        // Private members

#if NETFRAMEWORK

        private static Screenshot TakeScreenshot(this IWebDriver driver) {

            return ((ITakesScreenshot)UnwrapWebDriver(driver)).GetScreenshot();

        }
        private static Bitmap ScreenshotToBitmap(Screenshot screenshot) {

            using (Stream stream = new MemoryStream(screenshot.AsByteArray))
                return new Bitmap(stream);

        }

#endif

    }

}
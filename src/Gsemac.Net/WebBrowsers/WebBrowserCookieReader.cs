using System;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserCookieReader :
        IWebBrowserCookieReader {

        // Public members

        public WebBrowserCookieReader(IWebBrowserInfo webBrowserInfo) {

            this.webBrowserInfo = webBrowserInfo;

        }

        public CookieContainer GetCookies() => GetCookieReader(webBrowserInfo).GetCookies();
        public CookieCollection GetCookies(Uri uri) => GetCookieReader(webBrowserInfo).GetCookies(uri);

        // Private members

        private readonly IWebBrowserInfo webBrowserInfo;

        private IWebBrowserCookieReader GetCookieReader(IWebBrowserInfo webBrowserInfo) {

            switch (webBrowserInfo.Id) {

                case WebBrowserId.Chrome:
                    return new ChromeWebBrowserCookieReader();

                case WebBrowserId.Firefox:
                    return new FirefoxWebBrowserCookieReader();

                default:
                    throw new ArgumentException(nameof(webBrowserInfo));

            }

        }

    }

}
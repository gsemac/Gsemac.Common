using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal interface IWebBrowserCookiesReader {

        CookieContainer GetCookies(IWebBrowserProfile profile);

    }

}
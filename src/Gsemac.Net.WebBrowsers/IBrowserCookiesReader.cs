using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal interface IBrowserCookiesReader {

        CookieContainer GetCookies(IBrowserProfile profile);

    }

}
using System;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserCookieReader {

        CookieContainer GetCookies();
        CookieCollection GetCookies(Uri uri);

    }

}
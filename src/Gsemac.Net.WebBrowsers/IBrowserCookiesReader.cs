using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    internal interface IBrowserCookiesReader {

        IEnumerable<Cookie> GetCookies(IBrowserProfile profile);

    }

}
using System.Collections.Generic;
using System.Net;

namespace Gsemac.Net.WebBrowsers {

    public interface ICookiesReader {

        IEnumerable<Cookie> GetCookies();

    }

}
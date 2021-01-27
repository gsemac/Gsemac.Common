using Gsemac.IO.Logging;
using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverUpdater :
        ILoggable {

        IWebDriverInfo GetWebDriver(IWebBrowserInfo webBrowserInfo);

    }

}
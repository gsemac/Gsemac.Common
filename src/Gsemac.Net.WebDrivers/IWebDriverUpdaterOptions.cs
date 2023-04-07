using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverUpdaterOptions {

        BrowserId WebBrowserId { get; }
        string WebDriverDirectoryPath { get; }

    }

}
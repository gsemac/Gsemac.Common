using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverUpdaterOptions {

        WebBrowserId WebBrowserId { get; }
        string WebDriverDirectoryPath { get; }

    }

}
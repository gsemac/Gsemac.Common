using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverFactoryOptions {

        bool AutoUpdateEnabled { get; }
        bool IgnoreUpdateErrors { get; }
        WebBrowserId WebBrowserId { get; }
        IWebBrowserInfo DefaultWebBrowserInfo { get; }
        bool KillWebDriverProcessesOnDispose { get; }
        string WebDriverDirectoryPath { get; }

    }

}
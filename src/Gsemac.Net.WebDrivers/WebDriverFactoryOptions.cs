using Gsemac.Net.WebBrowsers;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverFactoryOptions :
        IWebDriverFactoryOptions {

        public bool AutoUpdateEnabled { get; set; } = true;
        public IWebBrowserInfo DefaultWebBrowser { get; set; }
        public bool KillWebDriverProcessesOnDispose { get; set; } = false;
        public string WebDriverDirectoryPath { get; set; }

        public static WebDriverFactoryOptions Default => new WebDriverFactoryOptions();

    }

}
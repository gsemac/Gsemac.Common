using Gsemac.Net.WebBrowsers;
using System;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverFactoryOptions :
        IWebDriverFactoryOptions {

        // Public members

        public bool AutoUpdateEnabled { get; set; } = true;
        public bool IgnoreUpdateErrors { get; set; } = true;
        public WebBrowserId WebBrowserId { get; set; } = WebBrowserId.Unknown;
        public IWebBrowserInfo DefaultWebBrowserInfo { get; set; }
        public bool KillWebDriverProcessesOnDispose { get; set; } = false;
        public string WebDriverDirectoryPath { get; set; }

        public static WebDriverFactoryOptions Default => new WebDriverFactoryOptions();

        public WebDriverFactoryOptions() { }
        public WebDriverFactoryOptions(IWebDriverFactoryOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            AutoUpdateEnabled = options.AutoUpdateEnabled;
            IgnoreUpdateErrors = options.IgnoreUpdateErrors;
            WebBrowserId = options.WebBrowserId;
            DefaultWebBrowserInfo = options.DefaultWebBrowserInfo;
            KillWebDriverProcessesOnDispose = options.KillWebDriverProcessesOnDispose;
            WebDriverDirectoryPath = options.WebDriverDirectoryPath;

        }

    }

}
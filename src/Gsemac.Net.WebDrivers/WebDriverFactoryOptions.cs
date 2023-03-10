using Gsemac.Net.WebBrowsers;
using System;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverFactoryOptions :
        IWebDriverFactoryOptions {

        // Public members

        public bool AutoUpdateEnabled { get; set; } = true;
        public bool IgnoreUpdateErrors { get; set; } = true;
        public WebBrowserId WebBrowserId {
            get => webBrowserId;
            set => webBrowserId = value;
        }
        public IWebBrowserInfo WebBrowser {
            get => webBrowserInfo;
            set {

                webBrowserInfo = value;

                if (webBrowserInfo is object)
                    WebBrowserId = webBrowserInfo.Id;

            }
        }
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
            WebBrowser = options.WebBrowser;
            KillWebDriverProcessesOnDispose = options.KillWebDriverProcessesOnDispose;
            WebDriverDirectoryPath = options.WebDriverDirectoryPath;

        }

        // Private members

        private WebBrowserId webBrowserId = WebBrowserId.Unknown;
        private IWebBrowserInfo webBrowserInfo;

    }

}
using Gsemac.Net.WebBrowsers;
using System;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverUpdaterOptions :
        IWebDriverUpdaterOptions {

        // Public members

        public WebBrowserId WebBrowserId { get; set; } = WebBrowserId.Unknown;
        public string WebDriverDirectoryPath { get; set; }

        public static WebDriverUpdaterOptions Default => new WebDriverUpdaterOptions();

        public WebDriverUpdaterOptions() {
        }
        public WebDriverUpdaterOptions(string webDriverDirectory) {

            WebDriverDirectoryPath = webDriverDirectory;

        }
        public WebDriverUpdaterOptions(IWebDriverUpdaterOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            WebBrowserId = options.WebBrowserId;
            WebDriverDirectoryPath = options.WebDriverDirectoryPath;

        }

    }

}
using System;
using System.Net;

namespace Gsemac.Net.WebDriverUtilities {

    public class WebDriverOptions :
        IWebDriverOptions {

        public IWebProxy Proxy { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public string UserAgent { get; set; }
        public string WebDriverExecutablePath { get; set; }
        public string BrowserExecutablePath { get; set; }
        public bool Headless { get; set; } = false;

    }

}
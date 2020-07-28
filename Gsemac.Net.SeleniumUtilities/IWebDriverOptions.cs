using System;
using System.Drawing;
using System.Net;

namespace Gsemac.Net.SeleniumUtilities {

    public interface IWebDriverOptions {

        IWebProxy Proxy { get; set; }
        TimeSpan Timeout { get; set; }
        string UserAgent { get; set; }
        string WebDriverExecutablePath { get; set; }
        string BrowserExecutablePath { get; set; }
        bool Headless { get; set; }
        Point WindowPosition { get; set; }
        Size WindowSize { get; set; }

    }

}
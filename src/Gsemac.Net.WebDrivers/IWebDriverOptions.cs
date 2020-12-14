using System;
using System.Drawing;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverOptions {

        Uri Uri { get; }
        IWebProxy Proxy { get; }
        TimeSpan Timeout { get; }
        string UserAgent { get; }
        string WebDriverExecutablePath { get; }
        bool Headless { get; }
        Point WindowPosition { get; }
        Size WindowSize { get; }

        PageLoadStrategy PageLoadStrategy { get; }
        bool DisablePopUps { get; }
        bool Stealth { get; }

    }

}
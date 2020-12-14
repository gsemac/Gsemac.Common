using System;
using System.Drawing;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverOptions :
        IWebDriverOptions {

        public Uri Uri { get; set; }
        public IWebProxy Proxy { get; set; } = WebProxyUtilities.GetDefaultProxy();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public string UserAgent { get; set; }
        public string WebDriverExecutablePath { get; set; }
        public bool Headless { get; set; } = false;
        public Point WindowPosition { get; set; } = new Point(0, 0);
        public Size WindowSize { get; set; } = new Size(1024, 768);

        public PageLoadStrategy PageLoadStrategy { get; set; } = PageLoadStrategy.Default;
        public bool DisablePopUps { get; set; } = false;
        public bool Stealth { get; set; } = false;

    }

}
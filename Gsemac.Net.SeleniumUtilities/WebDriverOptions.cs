using System;
using System.Drawing;
using System.Net;

namespace Gsemac.Net.SeleniumUtilities {

    public class WebDriverOptions :
        IWebDriverOptions {

        public IWebProxy Proxy { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public string UserAgent { get; set; }
        public string WebDriverExecutablePath { get; set; }
        public string BrowserExecutablePath { get; set; }
        public bool Headless { get; set; } = false;
        public Point WindowPosition { get; set; } = new Point(0, 0);
        public Size WindowSize { get; set; } = new Size(1024, 768);

        public PageLoadStrategy PageLoadStrategy { get; set; } = PageLoadStrategy.Default;
        public bool DisablePopUps { get; set; } = false;

    }

}
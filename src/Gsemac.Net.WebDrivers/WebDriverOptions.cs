using System;
using System.Drawing;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverOptions :
        IWebDriverOptions {

        public Uri Uri { get; set; }
        public IWebProxy Proxy { get; set; } = WebRequestUtilities.GetDefaultWebProxy();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public string UserAgent { get; set; }
        public string WebDriverExecutablePath { get; set; }
        public bool Headless { get; set; } = false;
        public Point WindowPosition { get; set; } = new Point(0, 0);
        public Size WindowSize { get; set; } = new Size(1024, 768);

        public PageLoadStrategy PageLoadStrategy { get; set; } = PageLoadStrategy.Default;
        public bool DisablePopUps { get; set; } = false;
        public bool Stealth { get; set; } = false;

        public static WebDriverOptions Default => new WebDriverOptions();

        public WebDriverOptions() { }
        public WebDriverOptions(IWebDriverOptions options) {

            Uri = options.Uri;
            Proxy = options.Proxy;
            Timeout = options.Timeout;
            UserAgent = options.UserAgent;
            WebDriverExecutablePath = options.WebDriverExecutablePath;
            Headless = options.Headless;
            WindowPosition = options.WindowPosition;
            WindowSize = options.WindowSize;

            PageLoadStrategy = options.PageLoadStrategy;
            DisablePopUps = options.DisablePopUps;
            Stealth = options.Stealth;

        }

    }

}
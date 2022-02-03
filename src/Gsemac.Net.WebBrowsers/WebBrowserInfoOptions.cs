namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfoOptions :
        IWebBrowserInfoOptions {

        // Public members

        public bool BypassCache { get; set; } = false;

        public static WebBrowserInfoOptions Default => new WebBrowserInfoOptions();

    }

}
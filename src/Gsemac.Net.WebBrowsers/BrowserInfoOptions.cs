namespace Gsemac.Net.WebBrowsers {

    public class BrowserInfoOptions :
        IBrowserInfoOptions {

        // Public members

        public bool BypassCache { get; set; } = false;

        public static BrowserInfoOptions Default => new BrowserInfoOptions();

    }

}
namespace Gsemac.Net.WebBrowsers {

    public class OpenUrlOptions :
        IOpenUrlOptions {

        // Public members

        public IWebBrowserInfo WebBrowser { get; set; }
        public IWebBrowserProfile Profile { get; set; }

        public static OpenUrlOptions Default => new OpenUrlOptions();

    }

}
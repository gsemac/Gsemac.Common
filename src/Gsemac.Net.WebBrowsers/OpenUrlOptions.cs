namespace Gsemac.Net.WebBrowsers {

    public class OpenUrlOptions :
        IOpenUrlOptions {

        // Public members

        public IWebBrowserInfo WebBrowser { get; set; }
        public WebBrowserId WebBrowserId { get; set; } = WebBrowserId.Unknown;
        public IWebBrowserProfile Profile { get; set; }
        public string UserDataDirectoryPath { get; set; }
        public bool PrivateMode { get; set; } = false;

        public static OpenUrlOptions Default => new OpenUrlOptions();

    }

}
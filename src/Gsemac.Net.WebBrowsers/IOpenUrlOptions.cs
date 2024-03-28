namespace Gsemac.Net.WebBrowsers {

    public interface IOpenUrlOptions {

        IWebBrowserInfo WebBrowser { get; }
        WebBrowserId WebBrowserId { get; }
        IWebBrowserProfile Profile { get; }
        string UserDataDirectoryPath { get; }

    }

}
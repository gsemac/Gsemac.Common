using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserInfoFactory {

        IWebBrowserInfo GetWebBrowserInfo(string browserExecutablePath);
        IWebBrowserInfo GetWebBrowserInfo(WebBrowserId browserId, IWebBrowserInfoOptions options);

        IWebBrowserInfo GetDefaultWebBrowser(IWebBrowserInfoOptions options);
        IEnumerable<IWebBrowserInfo> GetInstalledWebBrowsers(IWebBrowserInfoOptions options);

    }

}
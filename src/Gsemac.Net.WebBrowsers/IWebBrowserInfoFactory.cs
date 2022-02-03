using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserInfoFactory {

        IWebBrowserInfo GetInfo(string webBrowserExecutablePath);
        IWebBrowserInfo GetInfo(WebBrowserId webBrowserId, IWebBrowserInfoOptions options = null);

        IWebBrowserInfo GetDefaultWebBrowser();
        IEnumerable<IWebBrowserInfo> GetInstalledWebBrowsers(IWebBrowserInfoOptions options = null);

    }

}
using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserInfoFactory {

        IWebBrowserInfo GetInfo(string webBrowserExecutablePath);
        IWebBrowserInfo GetInfo(WebBrowserId webBrowserId, bool useCachedResult = true);

        IWebBrowserInfo GetDefaultWebBrowser();
        IEnumerable<IWebBrowserInfo> GetInstalledWebBrowsers(bool useCachedResult = true);

    }

}
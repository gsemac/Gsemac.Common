using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IBrowserInfoFactory {

        IBrowserInfo GetBrowserInfo(string webBrowserExecutablePath);
        IBrowserInfo GetBrowserInfo(BrowserId webBrowserId, IBrowserInfoOptions options);

        IBrowserInfo GetDefaultBrowser();
        IEnumerable<IBrowserInfo> GetInstalledBrowsers(IBrowserInfoOptions options);

    }

}